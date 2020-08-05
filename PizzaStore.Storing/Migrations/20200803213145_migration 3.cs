using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaStore.Storing.Migrations
{
    public partial class migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "size",
                table: "Pizzas");

            migrationBuilder.RenameColumn(
                name: "toppings",
                table: "Pizzas",
                newName: "Toppings");

            migrationBuilder.RenameColumn(
                name: "crust",
                table: "Pizzas",
                newName: "Crust");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "Pizzas",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Pizzas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreID = table.Column<int>(nullable: false),
                    PizzaID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Size = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Pizzas");

            migrationBuilder.RenameColumn(
                name: "Toppings",
                table: "Pizzas",
                newName: "toppings");

            migrationBuilder.RenameColumn(
                name: "Crust",
                table: "Pizzas",
                newName: "crust");

            migrationBuilder.AddColumn<string>(
                name: "size",
                table: "Pizzas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
