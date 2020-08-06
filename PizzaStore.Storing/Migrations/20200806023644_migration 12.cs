using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaStore.Storing.Migrations
{
    public partial class migration12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Crust",
                table: "Pizzas");

            migrationBuilder.AddColumn<int>(
                name: "DefaultCrustID",
                table: "Pizzas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CrustID",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Crust",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crust", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Crust");

            migrationBuilder.DropColumn(
                name: "DefaultCrustID",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "CrustID",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Crust",
                table: "Pizzas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
