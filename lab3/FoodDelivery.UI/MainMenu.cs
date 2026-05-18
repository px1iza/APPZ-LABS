using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Enum;


namespace FoodDelivery.UI.Menus;

public class MainMenu
{
    private readonly IDishService _dishService;
    private readonly IMenuService _menuService;
    private readonly IOrderService _orderService;

    public MainMenu(
        IDishService dishService,
        IMenuService menuService,
        IOrderService orderService)
    {
        _dishService = dishService;
        _menuService = menuService;
        _orderService = orderService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ClearConsole();
            PrintHeader("FOOD DELIVERY");

            Console.WriteLine("\nГОЛОВНЕ МЕНЮ\n");
            Console.WriteLine("1. Всі страви");
            Console.WriteLine("2. Страви за категорією");
            Console.WriteLine("3. Комплексний обід по днях");
            Console.WriteLine("4. Замовити окремі страви");
            Console.WriteLine("5. Замовити комплекс обід");
            Console.WriteLine("6. Пошук страви");
            Console.WriteLine("0. Вихід");

            Console.Write("\nВведіть опцію: ");
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    await ShowAllDishes();
                    break;

                case "2":
                    await ShowDishesByCategory();
                    break;

                case "3":
                    await ShowComplexByDays();
                    break;

                case "4":
                    await CreateOrder();
                    break;

                case "5":
                    await CreateComplexOrder();
                    break;

                case "6":
                    await SearchDishes();
                    break;

                case "0":
                    PrintSuccess("До побачення!");
                    return;

                default:
                    PrintError("Невірна опція. Спробуйте ще раз.");
                    Pause();
                    break;
            }
        }
    }

    private async Task ShowDishesByCategory()
    {
        while (true)
        {
            try
            {
                ClearConsole();
                PrintHeader("СТРАВИ ЗА КАТЕГОРІЄЮ");

                var categories = new[]
                {
                    (DishCategoryEnum.FirstCourses, "Перша страва (Супи)"),
                    (DishCategoryEnum.SecondCourses, "Друга страва (Гарніри)"),
                    (DishCategoryEnum.Drinks, "Напої"),
                    (DishCategoryEnum.Desserts, "Десерти"),
                    (DishCategoryEnum.ColdDishes, "Холодні страви (Салати)")
                };

                Console.WriteLine("\nВиберіть категорію:\n");

                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i].Item2}");
                }

                Console.WriteLine("0. Назад");

                Console.Write("\nВведіть категорію (1-5): ");

                if (!int.TryParse(Console.ReadLine()?.Trim(), out int choice) ||
                    choice < 0 || choice > categories.Length)
                {
                    PrintError("Невалідний вибір.");
                    Pause();
                    continue;
                }

                if (choice == 0)
                    break;

                var category = categories[choice - 1].Item1;

                var dishes = await _dishService.GetByCategoryAsync(category);

                if (dishes == null || !dishes.Any())
                {
                    PrintWarning("Страви цієї категорії не знайдено.");
                    Pause();
                    continue;
                }

                ClearConsole();
                PrintHeader(GetCategoryName(category).ToUpper());
                Console.WriteLine();
                PrintDishesTable(dishes);

                Pause();
            }
            catch (Exception ex)
            {
                PrintError($"Помилка: {ex.Message}");
                Pause();
            }
        }
    }

    private async Task ShowAllDishes()
    {
        try
        {
            ClearConsole();
            PrintHeader("ВСІ СТРАВИ");

            var dishes = await _dishService.GetAllAsync();

            if (dishes == null || !dishes.Any())
            {
                PrintWarning("Страви не знайдено в системі.");
                Pause();
                return;
            }

            PrintDishesTable(dishes);
        }
        catch (Exception ex)
        {
            PrintError($"Помилка: {ex.Message}");
        }

        Pause();
    }

    private async Task ShowComplexByDays()
    {
        try
        {
            ClearConsole();
            PrintHeader("КОМПЛЕКСНІ ОБІДИ ПО ДНЯХ");

            var complexes = await _menuService.GetAllComplexLunchesAsync();

            if (!complexes.Any())
            {
                PrintWarning("Комплексні обіди недоступні на жодний день.");
            }
            else
            {
                Console.WriteLine();
                foreach (var (day, dishes, originalPrice, discountedPrice, savings) in complexes)
                {
                    PrintComplexDay(day, dishes, originalPrice, discountedPrice, savings);
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            PrintError($"Помилка: {ex.Message}");
        }

        Pause();
    }

    private async Task SearchDishes()
    {
        try
        {
            ClearConsole();
            PrintHeader("ПОШУК СТРАВИ");

            Console.Write("\nВведіть назву страви: ");
            var keyword = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                PrintError("Введіть текст для пошуку.");
                Pause();
                return;
            }

            var results = await _dishService.SearchAsync(keyword);

            if (results == null || !results.Any())
            {
                PrintWarning($"Страви за запитом '{keyword}' не знайдено.");
                Pause();
                return;
            }

            Console.WriteLine($"\nРезультати пошуку за '{keyword}':\n");
            PrintDishesTable(results);
        }
        catch (Exception ex)
        {
            PrintError($"Помилка: {ex.Message}");
        }

        Pause();
    }

    private async Task CreateOrder()
    {
        try
        {
            ClearConsole();
            PrintHeader("ЗАМОВЛЕННЯ ОКРЕМИХ СТРАВ");

            var allDishes = await _dishService.GetAllAsync();
            if (allDishes == null || !allDishes.Any())
            {
                PrintWarning("Немає доступних страв.");
                Pause();
                return;
            }

            Console.WriteLine("\nДоступні страви:\n");
            foreach (var dish in allDishes)
            {
                Console.WriteLine($"  {dish.Id:D2}. {dish.Title,-25} {dish.Price,8:F2} грн");
            }

            var dishDict = new Dictionary<int, int>();

            Console.WriteLine();

            while (true)
            {
                Console.Write("Введіть ID страви (0 - готово): ");

                if (!int.TryParse(Console.ReadLine()?.Trim(), out int dishId))
                {
                    PrintError("Невірний ID.");
                    continue;
                }

                if (dishId == 0)
                {
                    if (!dishDict.Any())
                    {
                        PrintWarning("Замовлення скасовано.");
                        Pause();
                        return;
                    }
                    break;
                }

                var dish = allDishes.FirstOrDefault(d => d.Id == dishId);
                if (dish == null)
                {
                    PrintError($"Страва #{dishId} не знайдена.");
                    continue;
                }

                Console.Write($"Кількість для '{dish.Title}': ");

                if (!int.TryParse(Console.ReadLine()?.Trim(), out int quantity) || quantity < 1)
                {
                    PrintError("Кількість має бути від 1.");
                    continue;
                }

                if (dishDict.ContainsKey(dishId))
                    dishDict[dishId] += quantity;
                else
                    dishDict[dishId] = quantity;

                PrintSuccess($"Додано {quantity} x {dish.Title}");
                Console.WriteLine();
            }

            var (items, totalPrice) = await _orderService.GetOrderDetailsForDisplayAsync(dishDict);

            ClearConsole();
            PrintHeader("ПІДТВЕРДЖЕННЯ ЗАМОВЛЕННЯ");

            Console.WriteLine();
            foreach (var (title, quantity, price, itemTotal) in items)
            {
                Console.WriteLine($"  {title,-25} x{quantity} = {itemTotal,8:F2} грн");
            }

            Console.WriteLine();
            Console.WriteLine($"  РАЗОМ: {totalPrice,33:F2} грн");
            Console.WriteLine();

            Console.Write("Підтвердити замовлення? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                PrintWarning("Замовлення скасовано.");
                Pause();
                return;
            }

            var order = await _orderService.CreateOrderAsync(dishDict);

            ClearConsole();
            PrintHeader("ЗАМОВЛЕННЯ УСПІШНО СТВОРЕНО");

            Console.WriteLine();
            Console.WriteLine("Замовлення прийнято!");
            Console.WriteLine($"Сума до оплати: {order.TotalPrice,15:F2} грн");
            Console.WriteLine($"Час замовлення: {DateTime.Now:yyyy-MM-dd HH:mm}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            PrintError($"Помилка: {ex.Message}");
        }

        Pause();
    }

    private async Task CreateComplexOrder()
    {
        try
        {
            ClearConsole();
            PrintHeader("ЗАМОВЛЕННЯ КОМПЛЕКСНОГО ОБІДУ");

            var day = SelectDay();
            if (day == null)
            {
                PrintError("Операція скасована.");
                Pause();
                return;
            }

            var (dishes, originalPrice, discountedPrice, savings) =
                await _menuService.GetComplexLunchWithPriceAsync(day.Value);

            ClearConsole();
            PrintHeader($"КОМПЛЕКС НА {GetDayName(day.Value).ToUpper()}");

            PrintComplexDay(day.Value, dishes, originalPrice, discountedPrice, savings);

            Console.Write("\nЗамовити комплекс обід? (y/n): ");

            if (Console.ReadLine()?.ToLower() != "y")
            {
                PrintWarning("Замовлення скасовано.");
                Pause();
                return;
            }

            var order = await _orderService.CreateComplexLunchOrderAsync(day.Value);

            ClearConsole();
            PrintHeader("КОМПЛЕКС ЗАМОВЛЕНИЙ");

            Console.WriteLine();
            Console.WriteLine("Замовлення прийнято!");
            Console.WriteLine($"Сума до оплати: {order.TotalPrice,15:F2} грн");
            Console.WriteLine($"Час замовлення: {DateTime.Now:yyyy-MM-dd HH:mm}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            PrintError($"Помилка: {ex.Message}");
        }

        Pause();
    }

    private void PrintDishesTable(List<DishDTO> dishes)
    {
        if (dishes == null || !dishes.Any())
        {
            PrintWarning("Немає страв для показу.");
            return;
        }

        Console.WriteLine("ID  | Назва                    | Ціна грн | Категорія");
        Console.WriteLine("----+--------- ----------------+----------+-----------------");

        foreach (var dish in dishes)
        {
            string categoryName = GetCategoryName(dish.Category);
            Console.WriteLine($"{dish.Id,2}  | {dish.Title,-24} | {dish.Price,8:F2} | {categoryName,-17}");
        }

        Console.WriteLine();
    }

    private void PrintComplexDay(DayOfWeek day, List<DishDTO> dishes,
        decimal originalPrice, decimal discountedPrice, decimal savings)
    {
        string dayName = GetDayName(day);

        Console.WriteLine($"{dayName}\n");

        foreach (var dish in dishes)
        {
            Console.WriteLine($"  {dish.Title,-37} {dish.Price,8:F2} грн");
        }

        Console.WriteLine();
        Console.WriteLine($"  Оригінальна ціна:                        {originalPrice,8:F2} грн");
        Console.WriteLine($"  Знижка (15%):                        -{savings,8:F2} грн");
        Console.WriteLine($"  ЦІНА ЗІ ЗНИЖКОЮ:                         {discountedPrice,8:F2} грн");
        Console.WriteLine();
    }

    private DayOfWeek? SelectDay()
    {
        ClearConsole();
        PrintHeader("ВИБЕРІТЬ ДЕНЬ");

        Console.WriteLine("\n0. Неділя");
        Console.WriteLine("1. Понеділок");
        Console.WriteLine("2. Вівторок");
        Console.WriteLine("3. Середа");
        Console.WriteLine("4. Четвер");
        Console.WriteLine("5. П'ятниця");
        Console.WriteLine("6. Субота");

        Console.Write("\nВведіть день (0-6): ");

        if (!int.TryParse(Console.ReadLine()?.Trim(), out int dayNumber) || dayNumber < 0 || dayNumber > 6)
        {
            return null;
        }

        return (DayOfWeek)dayNumber;
    }

    private string GetDayName(DayOfWeek day) =>
        day switch
        {
            DayOfWeek.Sunday => "Неділя",
            DayOfWeek.Monday => "Понеділок",
            DayOfWeek.Tuesday => "Вівторок",
            DayOfWeek.Wednesday => "Середа",
            DayOfWeek.Thursday => "Четвер",
            DayOfWeek.Friday => "П'ятниця",
            DayOfWeek.Saturday => "Субота",
            _ => "Невідомий день"
        };

    private string GetCategoryName(DishCategoryEnum category) =>
        category switch
        {
            DishCategoryEnum.FirstCourses => "Перша страва",
            DishCategoryEnum.SecondCourses => "Друга страва",
            DishCategoryEnum.Drinks => "Напій",
            DishCategoryEnum.Desserts => "Десерт",
            DishCategoryEnum.ColdDishes => "Холодна страва",
            _ => "Інше"
        };

    private void PrintHeader(string title)
    {
        Console.WriteLine($"\n{title}\n");
    }

    private void PrintSuccess(string message)
    {
        Console.WriteLine(message);
    }

    private void PrintError(string message)
    {
        Console.WriteLine(message);
    }

    private void PrintWarning(string message)
    {
        Console.WriteLine(message);
    }

    private void ClearConsole()
    {
        Console.Clear();
    }

    private void Pause()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
        Console.ReadKey(true);
    }
}