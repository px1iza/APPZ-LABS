using FoodDelivery.DAL.Entities;
namespace FoodDelivery.BLL.DTO
{
    public class DishDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public DishCategory Category { get; set; }
    }
}