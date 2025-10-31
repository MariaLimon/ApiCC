using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "IsEmailConfirmed" },
                values: new object[] { "juan.perez@example.com", true });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "IsAdmin", "IsEmailConfirmed" },
                values: new object[] { "maria.gomez@example.com", true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "IsEmailConfirmed" },
                values: new object[] { "juan.perez@ejemplo.com", false });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "IsAdmin", "IsEmailConfirmed" },
                values: new object[] { "maria.gomez@ejemplo.com", false, false });
        }
    }
}
