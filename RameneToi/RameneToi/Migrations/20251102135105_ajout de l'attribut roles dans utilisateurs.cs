using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RameneToi.Migrations
{
    /// <inheritdoc />
    public partial class ajoutdelattributrolesdansutilisateurs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AlterColumn<float>(
                name: "Prix",
                table: "Composants",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Utilisateurs");

            migrationBuilder.AlterColumn<int>(
                name: "Prix",
                table: "Composants",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
