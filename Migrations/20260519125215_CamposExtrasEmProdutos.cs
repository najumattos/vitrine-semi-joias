using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitrineSemiJoias.Migrations
{
    /// <inheritdoc />
    public partial class CamposExtrasEmProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLastUnits",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "JewelryCode",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Profile", "SecurityStamp" },
                values: new object[] { "30f704d0-e010-4ea9-95af-48c3ef2c5255", "AQAAAAIAAYagAAAAELTh0HoboZ4Ba0YNL318Yeohi94Ww/vO+R1woZgEYJ7PW1c3QbBZDN9Sih5oug5Bfg==", "Client", "84a194f7-ae82-42b5-aa37-8fbf1785371a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLastUnits",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "JewelryCode",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Profile", "SecurityStamp" },
                values: new object[] { "8c2d41bd-2ac4-4182-8ddd-d664c8fb0cbf", "AQAAAAIAAYagAAAAEH+hRYaJITDGiH2bWqjPSi9sEZlBJfXyFhAoJ1+E5XNF2Vw4cG3JBi2Mtn5NcqsEQg==", "0", "6aec7b2c-cf66-4b36-9734-f5601351dce2" });
        }
    }
}
