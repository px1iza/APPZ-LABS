using FoodDelivery.BLL.DTO;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<List<DishDTO>> GetMenuByDayAsync(DayOfWeek day);

        Task<List<DishDTO>> GetComplexLunchAsync(DayOfWeek day);

        Task<List<DishDTO>> GetByCategoryAsync(DishCategoryDTO category);
    }
}