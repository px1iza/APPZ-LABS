using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Enum;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<List<DishDTO>> GetMenuByDayAsync(DayOfWeek day);
        Task<List<DishDTO>> GetComplexLunchAsync(DayOfWeek day);
        Task<(List<DishDTO> dishes, decimal originalPrice, decimal discountedPrice, decimal savings)>
            GetComplexLunchWithPriceAsync(DayOfWeek day);
        Task<List<(DayOfWeek day, List<DishDTO> dishes, decimal originalPrice, decimal discountedPrice, decimal savings)>>
            GetAllComplexLunchesAsync();
    }
}
