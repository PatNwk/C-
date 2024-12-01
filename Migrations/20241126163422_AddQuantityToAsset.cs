﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserAssetId",
                table: "UserAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Assets",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAssetId",
                table: "UserAssets");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Assets",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }
    }
}
