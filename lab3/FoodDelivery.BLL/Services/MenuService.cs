using AutoMapper;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Enum;

namespace FoodDelivery.BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private const decimal COMPLEX_DISCOUNT = 0.15m;

        public MenuService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<DishDTO>> GetMenuByDayAsync(DayOfWeek day)
        {
            var menuItems = await _uow.MenuItems.GetAllAsync();

            if (menuItems == null || !menuItems.Any())
                throw new Exception("Меню відсутнє в системі");

            var dishes = menuItems
                .Where(m => m.DayOfWeek == day && m.Dish != null)
                .Select(m => m.Dish)
                .Distinct()
                .ToList();

            if (!dishes.Any())
                throw new Exception($"Меню на {GetDayName(day)} не знайдено");

            return _mapper.Map<List<DishDTO>>(dishes);
        }

        public async Task<List<DishDTO>> GetComplexLunchAsync(DayOfWeek day)
        {
            var menu = await GetMenuByDayAsync(day);

            if (menu == null || !menu.Any())
                throw new Exception($"Меню на {GetDayName(day)} не знайдено");

            var groupedByCategory = menu
                .GroupBy(d => d.Category)
                .ToList();

            if (groupedByCategory.Count < 2)
                throw new Exception(
                    $"Комплексний обід недоступний на {GetDayName(day)} " +
                    $"(необхідно мінімум 2 категорії, є тільки {groupedByCategory.Count})");

            var result = new List<DishDTO>();

            foreach (var category in groupedByCategory.Take(3))
            {
                var dish = category.First();
                result.Add(dish);
            }

            if (result.Count < 2)
                throw new Exception("Не вдалося сформувати комплексний обід");

            return result;
        }

        public async Task<(List<DishDTO> dishes, decimal originalPrice, decimal discountedPrice, decimal savings)>
            GetComplexLunchWithPriceAsync(DayOfWeek day)
        {
            var dishes = await GetComplexLunchAsync(day);

            decimal originalPrice = dishes.Sum(d => d.Price);
            decimal discountedPrice = originalPrice * (1 - COMPLEX_DISCOUNT);
            decimal savings = originalPrice - discountedPrice;

            return (dishes, originalPrice, discountedPrice, savings);
        }

        public async Task<List<(DayOfWeek day, List<DishDTO> dishes, decimal originalPrice, decimal discountedPrice, decimal savings)>>
            GetAllComplexLunchesAsync()
        {
            var days = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                      DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday,
                      DayOfWeek.Sunday };

            var result = new List<(DayOfWeek, List<DishDTO>, decimal, decimal, decimal)>();

            foreach (var day in days)
            {
                try
                {
                    var (dishes, originalPrice, discountedPrice, savings) =
                        await GetComplexLunchWithPriceAsync(day);
                    result.Add((day, dishes, originalPrice, discountedPrice, savings));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Комплекс на {GetDayName(day)} недоступний: {ex.Message}");
                }
            }

            return result;
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
    }
}