namespace FoodDelivery.BLL.DTO
{
    public class OrderDTO
    {
        public DateTime OrderDate { get; set; }
        public bool IsComplex { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}