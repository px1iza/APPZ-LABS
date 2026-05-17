using FoodDelivery.BLL.DTO;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(List<int> dishIds);
    }
}