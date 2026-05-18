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
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalPrice)
                    .HasPrecision(10, 2);

                entity.Property(o => o.OrderDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(o => o.IsComplex)
                     .IsRequired()
                     .HasDefaultValue(false);

                entity.HasMany(o => o.OrderItems)
                    .WithOne()
                    .HasForeignKey("OrderId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

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

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.Property(mi => mi.DayOfWeek)
                    .IsRequired();

                entity.HasOne(mi => mi.Dish)
                    .WithMany()
                    .HasForeignKey(mi => mi.DishId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Dish>().HasData(
                new Dish { Id = 1, Title = "Борщ", Price = 120, Category = DishCategory.FirstCourses },
                new Dish { Id = 2, Title = "Суп-лапша", Price = 110, Category = DishCategory.FirstCourses },
                new Dish { Id = 3, Title = "Курка з рисом", Price = 130, Category = DishCategory.SecondCourses },
                new Dish { Id = 4, Title = "Стейк", Price = 150, Category = DishCategory.SecondCourses },
                new Dish { Id = 5, Title = "Рибка на гарнірі", Price = 140, Category = DishCategory.SecondCourses },
                new Dish { Id = 6, Title = "Чай", Price = 40, Category = DishCategory.Drinks },
                new Dish { Id = 7, Title = "Лимонад", Price = 50, Category = DishCategory.Drinks },
                new Dish { Id = 8, Title = "Компот", Price = 45, Category = DishCategory.Drinks },
                new Dish { Id = 9, Title = "Салат Цезар", Price = 100, Category = DishCategory.ColdDishes },
                new Dish { Id = 10, Title = "Тірамісу", Price = 85, Category = DishCategory.Desserts }
            );

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, DishId = 1, DayOfWeek = DayOfWeek.Monday },
                new MenuItem { Id = 2, DishId = 3, DayOfWeek = DayOfWeek.Monday },
                new MenuItem { Id = 3, DishId = 6, DayOfWeek = DayOfWeek.Monday },

                new MenuItem { Id = 4, DishId = 2, DayOfWeek = DayOfWeek.Tuesday },
                new MenuItem { Id = 5, DishId = 4, DayOfWeek = DayOfWeek.Tuesday },
                new MenuItem { Id = 6, DishId = 7, DayOfWeek = DayOfWeek.Tuesday },

                new MenuItem { Id = 7, DishId = 1, DayOfWeek = DayOfWeek.Wednesday },
                new MenuItem { Id = 8, DishId = 5, DayOfWeek = DayOfWeek.Wednesday },
                new MenuItem { Id = 9, DishId = 8, DayOfWeek = DayOfWeek.Wednesday },

                new MenuItem { Id = 10, DishId = 2, DayOfWeek = DayOfWeek.Thursday },
                new MenuItem { Id = 11, DishId = 3, DayOfWeek = DayOfWeek.Thursday },
                new MenuItem { Id = 12, DishId = 6, DayOfWeek = DayOfWeek.Thursday },
                new MenuItem { Id = 13, DishId = 9, DayOfWeek = DayOfWeek.Thursday },

                new MenuItem { Id = 14, DishId = 1, DayOfWeek = DayOfWeek.Friday },
                new MenuItem { Id = 15, DishId = 4, DayOfWeek = DayOfWeek.Friday },
                new MenuItem { Id = 16, DishId = 7, DayOfWeek = DayOfWeek.Friday },

                new MenuItem { Id = 17, DishId = 2, DayOfWeek = DayOfWeek.Saturday },
                new MenuItem { Id = 18, DishId = 5, DayOfWeek = DayOfWeek.Saturday },
                new MenuItem { Id = 19, DishId = 8, DayOfWeek = DayOfWeek.Saturday },
                new MenuItem { Id = 20, DishId = 10, DayOfWeek = DayOfWeek.Saturday },

                new MenuItem { Id = 21, DishId = 1, DayOfWeek = DayOfWeek.Sunday },
                new MenuItem { Id = 22, DishId = 3, DayOfWeek = DayOfWeek.Sunday },
                new MenuItem { Id = 23, DishId = 6, DayOfWeek = DayOfWeek.Sunday }
            );

        }
    }
}