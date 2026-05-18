using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Enum;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IDishService
    {
        Task<List<DishDTO>> GetAllAsync();
        Task<List<DishDTO>> GetByCategoryAsync(DishCategoryEnum category);
        Task<List<DishDTO>> SearchAsync(string keyword);
    }
}