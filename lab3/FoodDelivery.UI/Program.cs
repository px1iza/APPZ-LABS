using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Context;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FoodDelivery.BLL.Mapping;

namespace FoodDelivery.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var services = new ServiceCollection();

            // 🧾 DB CONTEXT
            services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(
            "server=localhost;port=3306;database=FoodDeliveryDb;user=root;password=root123",
            ServerVersion.AutoDetect("server=localhost;port=3306;database=FoodDeliveryDb;user=root;password=root123")
        )
    );

            // 📦 DAL
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 🧠 BLL SERVICES
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IOrderService, OrderService>();

            // 🔄 AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            var serviceProvider = services.BuildServiceProvider();

            // 🚀 запуск програми (як у FitnessClub ConsoleApp)
            var app = serviceProvider.GetRequiredService<ConsoleApp>();
            app.Run();
        }
    }
}