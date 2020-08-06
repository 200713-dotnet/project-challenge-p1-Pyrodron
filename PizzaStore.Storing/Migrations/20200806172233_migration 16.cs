using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaStore.Storing.Migrations
{
    public partial class migration16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Toppings",
                table: "Pizzas");

            migrationBuilder.AddColumn<string>(
                name: "DefaultToppings",
                table: "Pizzas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Toppings",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultToppings",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "Toppings",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Toppings",
                table: "Pizzas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
