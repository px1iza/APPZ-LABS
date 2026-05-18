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

        private const decimal COMPLEX_DISCOUNT = 0.15m; // 15% знижка

        public OrderService(
            IUnitOfWork uow,
            IMapper mapper,
            IMenuService menuService)
        {
            _uow = uow;
            _mapper = mapper;
            _menuService = menuService;
        }

        public async Task<OrderDTO> CreateOrderAsync(Dictionary<int, int> dishesWithQuantity)
        {
            if (dishesWithQuantity == null || !dishesWithQuantity.Any())
                throw new ArgumentException("Не вибрано жодної страви");

            foreach (var kvp in dishesWithQuantity)
            {
                if (kvp.Value < 1)
                    throw new ArgumentException($"Кількість для страви #{kvp.Key} має бути >= 1");
            }

            var dishes = await _uow.Dishes.GetAllAsync();

            if (dishes == null || !dishes.Any())
                throw new Exception("Страви не знайдено в системі");

            var selectedDishes = dishes
                .Where(d => dishesWithQuantity.Keys.Contains(d.Id))
                .ToList();

            if (selectedDishes.Count != dishesWithQuantity.Count)
            {
                var notFound = dishesWithQuantity.Keys
                    .Except(selectedDishes.Select(d => d.Id))
                    .ToList();
                throw new Exception($"Страви не знайдено: {string.Join(", ", notFound)}");
            }

            decimal totalPrice = 0;
            var orderItems = new List<OrderItem>();

            foreach (var dish in selectedDishes)
            {
                int quantity = dishesWithQuantity[dish.Id];
                totalPrice += dish.Price * quantity;

                orderItems.Add(new OrderItem
                {
                    DishId = dish.Id,
                    Quantity = quantity
                });
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                IsComplex = false,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            await _uow.Orders.AddAsync(order);
            await _uow.SaveAsync();

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateComplexLunchOrderAsync(DayOfWeek day)
        {
            var complexLunch = await _menuService.GetComplexLunchAsync(day);

            if (complexLunch == null || complexLunch.Count < 2)
                throw new Exception("Комплексний обід недоступний");

            decimal originalPrice = complexLunch.Sum(d => d.Price);
            decimal discountedPrice = originalPrice * (1 - COMPLEX_DISCOUNT);

            var orderItems = complexLunch
                .Select(d => new OrderItem
                {
                    DishId = d.Id,
                    Quantity = 1
                })
                .ToList();

            var order = new Order
            {
                OrderDate = DateTime.Now,
                IsComplex = true,
                TotalPrice = discountedPrice,
                OrderItems = orderItems
            };

            await _uow.Orders.AddAsync(order);
            await _uow.SaveAsync();

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<(List<(string DishTitle, int Quantity, decimal Price, decimal ItemTotal)> items, decimal totalPrice)>
            GetOrderDetailsForDisplayAsync(Dictionary<int, int> dishesWithQuantity)
        {
            if (dishesWithQuantity == null || !dishesWithQuantity.Any())
                throw new ArgumentException("Не вибрано жодної страви");

            var dishes = await _uow.Dishes.GetAllAsync();

            if (dishes == null || !dishes.Any())
                throw new Exception("Страви не знайдено в системі");

            var items = new List<(string, int, decimal, decimal)>();
            decimal totalPrice = 0;

            foreach (var kvp in dishesWithQuantity)
            {
                var dish = dishes.FirstOrDefault(d => d.Id == kvp.Key);
                if (dish == null)
                    throw new Exception($"Страва з ID {kvp.Key} не знайдена");

                decimal itemTotal = dish.Price * kvp.Value;
                totalPrice += itemTotal;
                items.Add((dish.Title, kvp.Value, dish.Price, itemTotal));
            }

            return (items, totalPrice);
        }
    }
}
