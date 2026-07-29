using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class OrderShippingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FullName", table: "Orders");
            migrationBuilder.DropColumn(name: "Phone", table: "Orders");
            migrationBuilder.DropColumn(name: "Address", table: "Orders");
            migrationBuilder.DropColumn(name: "City", table: "Orders");
            migrationBuilder.DropColumn(name: "Notes", table: "Orders");
        }
    }
}
