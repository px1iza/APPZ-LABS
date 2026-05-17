using AutoMapper;
using FoodDelivery.DAL.Entities;
using FoodDelivery.BLL.DTO;

namespace FoodDelivery.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 🍽 Dish → DishDTO
            CreateMap<Dish, DishDTO>();

            // 🧾 Order → OrderDTO
            CreateMap<Order, OrderDTO>();

            // 🍴 OrderItem → OrderItemDTO
            CreateMap<OrderItem, OrderItemDTO>();
        }
    }
}