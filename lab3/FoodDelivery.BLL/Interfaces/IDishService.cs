using FoodDelivery.BLL.DTO;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IDishService
    {
        Task<List<DishDTO>> GetAllAsync();

        Task<List<DishDTO>> GetByCategoryAsync(DishCategory category);

        Task<List<DishDTO>> GetByDayAsync(DayOfWeek day);
    }
}