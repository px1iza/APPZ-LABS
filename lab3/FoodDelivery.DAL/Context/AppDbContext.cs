using FoodDelivery.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🍽 Dish
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(d => d.Title)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(d => d.Price)
                    .HasPrecision(10, 2);

                entity.Property(d => d.Category)
                    .IsRequired();

                entity.Property(d => d.DayOfWeek)
                    .IsRequired();
            });

            // 🧾 Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalPrice)
                    .HasPrecision(10, 2);

                entity.Property(o => o.OrderDate)
                    .IsRequired();

                entity.Property(o => o.IsComplex)
                    .IsRequired();

                entity.HasMany(o => o.OrderItems)
                    .WithOne()
                    .HasForeignKey("OrderId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 🍴 OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(oi => oi.Quantity)
                    .IsRequired()
                    .HasDefaultValue(1);

                entity.HasOne(oi => oi.Dish)
                    .WithMany()
                    .HasForeignKey(oi => oi.DishId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // 🌱 START DATA
            modelBuilder.Entity<Dish>().HasData(

                new Dish
                {
                    Id = 1,
                    Title = "Борщ",
                    Price = 120,
                    DayOfWeek = DayOfWeek.Monday,
                    Category = DishCategory.FirstCourses
                },

                new Dish
                {
                    Id = 2,
                    Title = "Смажена картопля",
                    Price = 110,
                    DayOfWeek = DayOfWeek.Monday,
                    Category = DishCategory.SecondCourses
                },

                new Dish
                {
                    Id = 3,
                    Title = "Вареники",
                    Price = 130,
                    DayOfWeek = DayOfWeek.Tuesday,
                    Category = DishCategory.SecondCourses
                },

                new Dish
                {
                    Id = 4,
                    Title = "Салат Цезар",
                    Price = 140,
                    DayOfWeek = DayOfWeek.Wednesday,
                    Category = DishCategory.ColdDishes
                },

                new Dish
                {
                    Id = 5,
                    Title = "Чай",
                    Price = 40,
                    DayOfWeek = DayOfWeek.Monday,
                    Category = DishCategory.Drinks
                },

                new Dish
                {
                    Id = 6,
                    Title = "Кока-Кола",
                    Price = 55,
                    DayOfWeek = DayOfWeek.Friday,
                    Category = DishCategory.Drinks
                },

                new Dish
                {
                    Id = 7,
                    Title = "Вода",
                    Price = 30,
                    DayOfWeek = DayOfWeek.Saturday,
                    Category = DishCategory.Drinks
                }
            );
        }
    }
}