using AutoMapper;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Interfaces;

namespace FoodDelivery.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<OrderDTO> CreateOrderAsync(List<int> dishIds)
        {
            if (dishIds == null || !dishIds.Any())
                throw new Exception("Не вибрано жодної страви");

            var dishes = await _uow.Dishes.GetAllAsync();

            var selected = dishes
                .Where(d => dishIds.Contains(d.Id))
                .ToList();

            if (selected.Count != dishIds.Count)
                throw new Exception("Деякі страви не знайдено");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                IsComplex = false,
                TotalPrice = selected.Sum(d => d.Price),
                OrderItems = selected.Select(d => new OrderItem
                {
                    DishId = d.Id,
                    Quantity = 1
                }).ToList()
            };

            await _uow.Orders.AddAsync(order);
            await _uow.SaveAsync();

            return _mapper.Map<OrderDTO>(order);
        }
    }
}