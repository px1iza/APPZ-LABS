using FoodDelivery.DAL.Entities;
namespace FoodDelivery.DAL.Interfaces;

public interface IUnitOfWork
{
    IRepository<Dish> Dishes { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderItem> OrderItems { get; }
    IRepository<MenuItem> MenuItems { get; }
    Task SaveAsync();
}