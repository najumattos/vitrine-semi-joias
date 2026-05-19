using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitrineSemiJoias.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLastUnits",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Profile", "SecurityStamp" },
                values: new object[] { "68958a0f-209a-426a-9179-e7dc55438e86", "AQAAAAIAAYagAAAAEJUcXLoVZioGgNc+8MmqqZHJt0DMsGGuXxZowl9oOkAATvRQ5Ro3QK4BU7C9Pbi/Hw==", "Client", "03068a27-3622-49bc-94c0-7fb6f2cb6922" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryEnum", "Description", "ImageUrl", "IsAvailable", "JewelryCode", "Price", "Title" },
                values: new object[] { 1, "Anel", "Anel solitário clássico confeccionado em prata 925 com pedra de zircônia central.", "wwwroot/img/seedAnel", true, 1001, 129.90m, "Anel Solitário de Prata Zircônia" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_JewelryCode",
                table: "Products",
                column: "JewelryCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_JewelryCode",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "IsLastUnits",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Profile", "SecurityStamp" },
                values: new object[] { "30f704d0-e010-4ea9-95af-48c3ef2c5255", "AQAAAAIAAYagAAAAELTh0HoboZ4Ba0YNL318Yeohi94Ww/vO+R1woZgEYJ7PW1c3QbBZDN9Sih5oug5Bfg==", "0", "84a194f7-ae82-42b5-aa37-8fbf1785371a" });
        }
    }
}
