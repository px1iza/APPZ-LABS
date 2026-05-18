using FoodDelivery.IoC;
using FoodDelivery.UI.Menus;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();

services.AddFoodDelivery();

services.AddTransient<MainMenu>();

var provider = services.BuildServiceProvider();

var menu = provider.GetRequiredService<MainMenu>();

await menu.RunAsync();