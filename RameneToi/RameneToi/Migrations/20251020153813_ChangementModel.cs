using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RameneToi.Migrations
{
    /// <inheritdoc />
    public partial class ChangementModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_ConfigurationPcs_ConfigurationPcId",
                table: "Commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Utilisateurs_UtilisateurId",
                table: "Commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_est_composé_de_Composants_ComposantsId",
                table: "est_composé_de");

            migrationBuilder.DropForeignKey(
                name: "FK_est_composé_de_ConfigurationPcs_ConfigurationsId",
                table: "est_composé_de");

            migrationBuilder.DropForeignKey(
                name: "FK_Adresses_Utilisateurs",
                table: "Utilisateurs");

            migrationBuilder.DropIndex(
                name: "IX_Commandes_UtilisateurId",
                table: "Commandes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_est_composé_de",
                table: "est_composé_de");

            migrationBuilder.RenameTable(
                name: "est_composé_de",
                newName: "ComposantConfigurationPc");

            migrationBuilder.RenameIndex(
                name: "IX_est_composé_de_ConfigurationsId",
                table: "ComposantConfigurationPc",
                newName: "IX_ComposantConfigurationPc_ConfigurationsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComposantConfigurationPc",
                table: "ComposantConfigurationPc",
                columns: new[] { "ComposantsId", "ConfigurationsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_ConfigurationPcs_ConfigurationPcId",
                table: "Commandes",
                column: "ConfigurationPcId",
                principalTable: "ConfigurationPcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComposantConfigurationPc_Composants_ComposantsId",
                table: "ComposantConfigurationPc",
                column: "ComposantsId",
                principalTable: "Composants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComposantConfigurationPc_ConfigurationPcs_ConfigurationsId",
                table: "ComposantConfigurationPc",
                column: "ConfigurationsId",
                principalTable: "ConfigurationPcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Adresses_AdresseId",
                table: "Utilisateurs",
                column: "AdresseId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_ConfigurationPcs_ConfigurationPcId",
                table: "Commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_ComposantConfigurationPc_Composants_ComposantsId",
                table: "ComposantConfigurationPc");

            migrationBuilder.DropForeignKey(
                name: "FK_ComposantConfigurationPc_ConfigurationPcs_ConfigurationsId",
                table: "ComposantConfigurationPc");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Adresses_AdresseId",
                table: "Utilisateurs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComposantConfigurationPc",
                table: "ComposantConfigurationPc");

            migrationBuilder.RenameTable(
                name: "ComposantConfigurationPc",
                newName: "est_composé_de");

            migrationBuilder.RenameIndex(
                name: "IX_ComposantConfigurationPc_ConfigurationsId",
                table: "est_composé_de",
                newName: "IX_est_composé_de_ConfigurationsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_est_composé_de",
                table: "est_composé_de",
                columns: new[] { "ComposantsId", "ConfigurationsId" });

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_UtilisateurId",
                table: "Commandes",
                column: "UtilisateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_ConfigurationPcs_ConfigurationPcId",
                table: "Commandes",
                column: "ConfigurationPcId",
                principalTable: "ConfigurationPcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Utilisateurs_UtilisateurId",
                table: "Commandes",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_est_composé_de_Composants_ComposantsId",
                table: "est_composé_de",
                column: "ComposantsId",
                principalTable: "Composants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_composé_de_ConfigurationPcs_ConfigurationsId",
                table: "est_composé_de",
                column: "ConfigurationsId",
                principalTable: "ConfigurationPcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Adresses_Utilisateurs",
                table: "Utilisateurs",
                column: "AdresseId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
