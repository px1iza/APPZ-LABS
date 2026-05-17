namespace FoodDelivery.DAL.Entities;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; }
    public bool IsComplex { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public decimal TotalPrice { get; set; }
}