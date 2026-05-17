namespace FoodDelivery.DAL.Entities;

public class Dish : BaseEntity
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public DishCategory Category { get; set; }
}