using Microsoft.EntityFrameworkCore.Migrations;

namespace MiWebApiM3.Migrations
{
    public partial class admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4965866a-38ed-405a-8003-45c52adf08ab", "90e02813-b137-4f3e-b1ad-53070e0bdc1b", "admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4965866a-38ed-405a-8003-45c52adf08ab");
        }
    }
}
