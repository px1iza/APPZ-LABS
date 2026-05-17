namespace FoodDelivery.DAL.Entities;

public class OrderItem : BaseEntity
{
    public int DishId { get; set; }
    public Dish Dish { get; set; } = null!;
    public int Quantity { get; set; }
}