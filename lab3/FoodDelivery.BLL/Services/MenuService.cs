using AutoMapper;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Interfaces;

namespace FoodDelivery.BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MenuService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // 📅 Меню на конкретний день

        public async Task<List<DishDTO>> GetMenuByDayAsync(DayOfWeek day)
        {

            var menuItems = await _uow.MenuItems.GetAllAsync();

            var dishes = menuItems

                .Where(m => m.DayOfWeek == day)

                .Select(m => m.Dish)

                .Distinct()

                .ToList();

            return _mapper.Map<List<DishDTO>>(dishes);

        }
        // 🍱 Комплексний обід
        // (1 перша страва + 1 друга + 1 напій)
        public async Task<List<DishDTO>> GetComplexLunchAsync(DayOfWeek day)
        {
            var menu = await GetMenuByDayAsync(day);

            if (!menu.Any())

                throw new Exception("Меню на цей день відсутнє");

            var result = new List<DishDTO>();

            var firstCourse = menu

                .FirstOrDefault(d => d.Category == DishCategory.FirstCourses);

            var secondCourse = menu

                .FirstOrDefault(d => d.Category == DishCategory.SecondCourses);

            var drink = menu

                .FirstOrDefault(d => d.Category == DishCategory.Drinks);

            if (firstCourse != null)

                result.Add(firstCourse);

            if (secondCourse != null)

                result.Add(secondCourse);

            if (drink != null)

                result.Add(drink);

            if (!result.Any())

                throw new Exception("Неможливо сформувати комплексний обід");

            return result;

        }
        // 🔎 Фільтр по категорії в межах дня

        public async Task<List<DishDTO>> GetByCategoryAndDayAsync(
            DayOfWeek day,
            DishCategory category)
        {
            var menu = await GetMenuByDayAsync(day);
            var filtered = menu
                .Where(d => d.Category == category)
                .ToList();
            return filtered;
        }
    }
}