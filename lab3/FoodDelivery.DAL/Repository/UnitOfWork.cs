using FoodDelivery.DAL.Context;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.Interfaces;

namespace FoodDelivery.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<Dish> Dishes { get; }
        public IRepository<Order> Orders { get; }
        public IRepository<OrderItem> OrderItems { get; }

        public UnitOfWork(
            AppDbContext context,
            IRepository<Dish> dishes,
            IRepository<Order> orders,
            IRepository<OrderItem> orderItems)
        {
            _context = context;

            Dishes = dishes;
            Orders = orders;
            OrderItems = orderItems;
        }

        public async Task SaveAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}