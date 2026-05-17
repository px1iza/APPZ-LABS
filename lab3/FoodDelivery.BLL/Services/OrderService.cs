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
        private readonly IMenuService _menuService;

        public OrderService(
            IUnitOfWork uow,
            IMapper mapper,
            IMenuService menuService)
        {
            _uow = uow;
            _mapper = mapper;
            _menuService = menuService;
        }

        // 🍽 замовлення окремих страв
        public async Task<OrderDTO> CreateOrderAsync(List<int> dishIds)
        {
            if (dishIds == null || !dishIds.Any())
                throw new Exception("Не вибрано жодної страви");

            var dishes = await _uow.Dishes.GetAllAsync();

            var selectedDishes = dishes
                .Where(d => dishIds.Contains(d.Id))
                .ToList();

            if (selectedDishes.Count != dishIds.Count)
                throw new Exception("Деякі страви не знайдено");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                IsComplex = false,

                TotalPrice = selectedDishes.Sum(d => d.Price),

                OrderItems = selectedDishes
                    .Select(d => new OrderItem
                    {
                        DishId = d.Id,
                        Quantity = 1
                    })
                    .ToList()
            };

            await _uow.Orders.AddAsync(order);
            await _uow.SaveAsync();

            return _mapper.Map<OrderDTO>(order);
        }

        // 🍱 замовлення комплексного обіду
        public async Task<OrderDTO> CreateComplexLunchOrderAsync(
            DayOfWeek day)
        {
            var complexLunch =
                await _menuService.GetComplexLunchAsync(day);

            if (!complexLunch.Any())
                throw new Exception(
                    "Комплексний обід недоступний");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                IsComplex = true,

                TotalPrice = complexLunch.Sum(d => d.Price),

                OrderItems = complexLunch
                    .Select(d => new OrderItem
                    {
                        DishId = d.Id,
                        Quantity = 1
                    })
                    .ToList()
            };

            await _uow.Orders.AddAsync(order);
            await _uow.SaveAsync();

            return _mapper.Map<OrderDTO>(order);
        }
    }
}