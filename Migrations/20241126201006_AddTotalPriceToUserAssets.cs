using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonWebApp.Migrations
{
    public partial class AddTotalPriceToUserAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ajouter la colonne TotalPrice à UserAssets sans toucher à la clé primaire
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "UserAssets",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            // Si nécessaire, vous pouvez également ajouter un index sur la colonne UserId
            migrationBuilder.CreateIndex(
                name: "IX_UserAssets_UserId",
                table: "UserAssets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Supprimer la colonne TotalPrice si la migration est annulée
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "UserAssets");

            // Supprimer l'index UserId si nécessaire
            migrationBuilder.DropIndex(
                name: "IX_UserAssets_UserId",
                table: "UserAssets");
        }
    }
}
