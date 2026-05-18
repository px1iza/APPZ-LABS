using AutoMapper;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Mapping;
using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Context;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodDelivery.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddFoodDelivery(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                "server=localhost;database=FoodDeliveryDb;user=root;password=root123;",
                ServerVersion.AutoDetect("server=localhost;database=FoodDeliveryDb;user=root;password=root123;")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(M => M.AddMaps(typeof(MappingProfile).Assembly));

        services.AddScoped<IDishService, DishService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}