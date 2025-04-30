using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace examDotNet.Migrations
{
    /// <inheritdoc />
    public partial class AddNbStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NbStock",
                table: "Produits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NbStock",
                table: "Produits");
        }
    }
}
