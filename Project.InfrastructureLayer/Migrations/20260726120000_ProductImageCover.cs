using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class ProductImageCover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCover",
                table: "ProductImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCover",
                table: "ProductImages");
        }
    }
}
