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

        // ➕ НОВЕ
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🍽 DISH (залишаємо як є)
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(d => d.Title)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(d => d.Price)
                    .HasPrecision(10, 2);

                entity.Property(d => d.Category)
                    .IsRequired();
            });

            // 🧾 ORDER (без змін)
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

            // 🍴 ORDER ITEM (без змін)
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

            // 📅 MENU ITEM (НОВЕ — ключова частина)
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.Property(mi => mi.DayOfWeek)
                    .IsRequired();

                entity.HasOne(mi => mi.Dish)
                    .WithMany()
                    .HasForeignKey(mi => mi.DishId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 🌱 DISH SEED (залишив як у тебе)
            modelBuilder.Entity<Dish>().HasData(
                new Dish { Id = 1, Title = "Борщ", Price = 120, Category = DishCategory.FirstCourses },
                new Dish { Id = 2, Title = "Смажена картопля", Price = 110, Category = DishCategory.SecondCourses },
                new Dish { Id = 3, Title = "Вареники", Price = 130, Category = DishCategory.SecondCourses },
                new Dish { Id = 4, Title = "Салат Цезар", Price = 140, Category = DishCategory.ColdDishes },
                new Dish { Id = 5, Title = "Чай", Price = 40, Category = DishCategory.Drinks },
                new Dish { Id = 6, Title = "Кава", Price = 55, Category = DishCategory.Drinks },
                new Dish { Id = 7, Title = "Вода", Price = 30, Category = DishCategory.Drinks }
            );

            // 🍱 MENU SEED (НОВЕ — це і є твоє меню)
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, DishId = 1, DayOfWeek = DayOfWeek.Monday },
                new MenuItem { Id = 2, DishId = 5, DayOfWeek = DayOfWeek.Monday },

                new MenuItem { Id = 3, DishId = 2, DayOfWeek = DayOfWeek.Tuesday },
                new MenuItem { Id = 4, DishId = 3, DayOfWeek = DayOfWeek.Tuesday },

                new MenuItem { Id = 5, DishId = 4, DayOfWeek = DayOfWeek.Wednesday },

                new MenuItem { Id = 6, DishId = 6, DayOfWeek = DayOfWeek.Friday },

                new MenuItem { Id = 7, DishId = 7, DayOfWeek = DayOfWeek.Saturday }
            );
        }
    }
}