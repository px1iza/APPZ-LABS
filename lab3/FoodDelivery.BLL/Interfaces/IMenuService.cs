using FoodDelivery.BLL.DTO;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IMenuService
    {
        // 📅 меню на день
        Task<List<DishDTO>> GetMenuByDayAsync(DayOfWeek day);

        // 🍱 комплексний обід
        Task<List<DishDTO>> GetComplexLunchAsync(DayOfWeek day);

        // 🔎 фільтр по категорії в межах дня
        Task<List<DishDTO>> GetByCategoryAndDayAsync(
            DayOfWeek day,
            DishCategory category);
    }
}