using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RameneToi.Migrations
{
    /// <inheritdoc />
    public partial class ModifDesmodelsrelationstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_AdresseId",
                table: "Utilisateurs");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_AdresseId",
                table: "Utilisateurs",
                column: "AdresseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_AdresseId",
                table: "Utilisateurs");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_AdresseId",
                table: "Utilisateurs",
                column: "AdresseId",
                unique: true);
        }
    }
}
