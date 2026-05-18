using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FoodDelivery.DAL.Context;

namespace FoodDelivery.DAL.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseMySql(
                "server=localhost;database=FoodDeliveryDb;user=root;password=root123;",
                ServerVersion.AutoDetect("server=localhost;database=FoodDeliveryDb;user=root;password=root123;")
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}