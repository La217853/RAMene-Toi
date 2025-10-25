using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RameneToi.Migrations
{
    /// <inheritdoc />
    public partial class RetirerleNomdanscomposantmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Composants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Composants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
