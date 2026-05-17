namespace FoodDelivery.DAL.Entities
{
    public class MenuItem : BaseEntity
    {
        public DayOfWeek DayOfWeek { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}