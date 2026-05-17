namespace FoodDelivery.BLL.DTO
{
    public class OrderItemDTO
    {
        public int DishId { get; set; }
        public string DishTitle { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }
}