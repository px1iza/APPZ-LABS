using AutoMapper;
using FoodDelivery.DAL.Entities;
using FoodDelivery.BLL.DTO;

namespace FoodDelivery.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Dish, DishDTO>();

            CreateMap<Order, OrderDTO>();

            CreateMap<OrderItem, OrderItemDTO>();
        }
    }
}