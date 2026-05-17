using FoodDelivery.BLL.DTO;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IOrderService
    {
        // 🍽 окремі страви
        Task<OrderDTO> CreateOrderAsync(List<int> dishIds);

        // 🍱 комплексний обід
        Task<OrderDTO> CreateComplexLunchOrderAsync(
            DayOfWeek day);
    }
}