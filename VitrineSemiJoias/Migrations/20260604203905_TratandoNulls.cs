using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitrineSemiJoias.Migrations
{
    /// <inheritdoc />
    public partial class TratandoNulls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e058c5bb-fc93-48b4-84e0-d5978520b10d", "AQAAAAIAAYagAAAAECzUnpK24oA7S2iRzJeJuc1Z2rfiec4ragUACGxfyjR9tuV0R1cPIejJZLGDa0D6IQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cc23197a-9ef6-4354-a025-80794209dbc2", "AQAAAAIAAYagAAAAECBW7yPYNlTelZ3MWpjLSIzBj4exYEtp/au1KXJDA0jmrY25mqJzGWBvXNp8YL5LJw==" });
        }
    }
}
