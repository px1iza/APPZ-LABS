using FoodDelivery.BLL.DTO;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(Dictionary<int, int> dishesWithQuantity);
        Task<OrderDTO> CreateComplexLunchOrderAsync(DayOfWeek day);
        Task<(List<(string DishTitle, int Quantity, decimal Price, decimal ItemTotal)> items, decimal totalPrice)>
            GetOrderDetailsForDisplayAsync(Dictionary<int, int> dishesWithQuantity);
    }
}
