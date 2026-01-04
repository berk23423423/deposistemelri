using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepoEnvanterApp.Migrations
{
    /// <inheritdoc />
    public partial class KategoriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kategori",
                table: "Urunler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kategori",
                table: "Urunler");
        }
    }
}
