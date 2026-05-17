namespace FoodDelivery.BLL.DTO
{
    public class DishDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DishCategoryDTO Category { get; set; }
    }
}