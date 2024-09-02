using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "CreatedOn", "Email", "IsAdmin", "Name", "PasswordHash", "PasswordSalt", "Phone", "Status", "UpdatedOn" },
                values: new object[] { new Guid("d08e62fd-4ecb-4c04-b8ae-ab50094eeb91"), "LDC", new DateTime(2024, 8, 26, 11, 17, 3, 930, DateTimeKind.Unspecified), "farouq@admin.com", true, "SuperAdmin", "D5QKIZDafAB4nsYxXEqcmxELEFM5EGliFOAwIuZ/B6o=", "U5KJucTPYfhhl9avWxEBcdV0z9ZimJ2jj19swLCUpIw=", "01111111111", "InActive", new DateTime(2024, 8, 26, 11, 17, 3, 930, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "CreatedOn", "Email", "Name", "PasswordHash", "PasswordSalt", "Phone", "Status", "UpdatedOn" },
                values: new object[] { new Guid("f005e3b3-9385-4a29-9d1f-3b5371d23d99"), "I live here", new DateTime(2024, 9, 1, 21, 55, 55, 406, DateTimeKind.Unspecified).AddTicks(6667), "firstuser@example.com", "First User", "D5QKIZDafAB4nsYxXEqcmxELEFM5EGliFOAwIuZ/B6o=", "U5KJucTPYfhhl9avWxEBcdV0z9ZimJ2jj19swLCUpIw=", "+201111111111", "InActive", new DateTime(2024, 9, 1, 21, 55, 55, 406, DateTimeKind.Unspecified).AddTicks(6667) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("d08e62fd-4ecb-4c04-b8ae-ab50094eeb91"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("f005e3b3-9385-4a29-9d1f-3b5371d23d99"));
        }
    }
}
