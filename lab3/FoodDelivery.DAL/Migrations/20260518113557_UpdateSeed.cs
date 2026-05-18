using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDelivery.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Title" },
                values: new object[] { 0, "Суп-лапша" });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Курка з рисом");

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Price", "Title" },
                values: new object[] { 1, 150m, "Стейк" });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Price", "Title" },
                values: new object[] { 1, 140m, "Рибка на гарнірі" });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Price", "Title" },
                values: new object[] { 40m, "Чай" });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Price", "Title" },
                values: new object[] { 50m, "Лимонад" });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Price", "Title" },
                values: new object[,]
                {
                    { 8, 3, 45m, "Компот" },
                    { 9, 2, 100m, "Салат Цезар" },
                    { 10, 4, 85m, "Тірамісу" }
                });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DishId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DayOfWeek", "DishId" },
                values: new object[] { 1, 6 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DishId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "DayOfWeek",
                value: 2);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DayOfWeek", "DishId" },
                values: new object[] { 2, 7 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DayOfWeek", "DishId" },
                values: new object[] { 3, 1 });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "DayOfWeek", "DishId" },
                values: new object[,]
                {
                    { 8, 3, 5 },
                    { 10, 4, 2 },
                    { 11, 4, 3 },
                    { 12, 4, 6 },
                    { 14, 5, 1 },
                    { 15, 5, 4 },
                    { 16, 5, 7 },
                    { 17, 6, 2 },
                    { 18, 6, 5 },
                    { 21, 0, 1 },
                    { 22, 0, 3 },
                    { 23, 0, 6 },
                    { 9, 3, 8 },
                    { 13, 4, 9 },
                    { 19, 6, 8 },
                    { 20, 6, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Title" },
                values: new object[] { 1, "Смажена картопля" });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Вареники");

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Price", "Title" },
                values: new object[] { 2, 140m, "Салат Цезар" });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Price", "Title" },
                values: new object[] { 3, 40m, "Чай" });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Price", "Title" },
                values: new object[] { 55m, "Кава" });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Price", "Title" },
                values: new object[] { 30m, "Вода" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "DishId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DayOfWeek", "DishId" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "DishId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "DayOfWeek",
                value: 3);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DayOfWeek", "DishId" },
                values: new object[] { 5, 6 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DayOfWeek", "DishId" },
                values: new object[] { 6, 7 });
        }
    }
}
