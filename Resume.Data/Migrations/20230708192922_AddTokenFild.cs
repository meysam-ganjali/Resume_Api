using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resume.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenFild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e51f23bf-bbd5-4beb-ad8e-65100792f034"));

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("444b1aa7-9477-4d72-b547-e3a062fd8add"), new DateTime(2023, 7, 8, 12, 29, 22, 601, DateTimeKind.Local).AddTicks(816), "Administrator", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("444b1aa7-9477-4d72-b547-e3a062fd8add"));

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("e51f23bf-bbd5-4beb-ad8e-65100792f034"), new DateTime(2023, 7, 8, 7, 13, 36, 607, DateTimeKind.Local).AddTicks(2428), "Administrator", null });
        }
    }
}
