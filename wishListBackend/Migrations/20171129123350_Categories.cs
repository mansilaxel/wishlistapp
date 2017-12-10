using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace wishListBackend.Migrations
{
    public partial class Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WishCategoryId",
                table: "Wish",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WishCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naam = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishCategory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wish_WishCategoryId",
                table: "Wish",
                column: "WishCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WishCategory_UserId",
                table: "WishCategory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wish_WishCategory_WishCategoryId",
                table: "Wish",
                column: "WishCategoryId",
                principalTable: "WishCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wish_WishCategory_WishCategoryId",
                table: "Wish");

            migrationBuilder.DropTable(
                name: "WishCategory");

            migrationBuilder.DropIndex(
                name: "IX_Wish_WishCategoryId",
                table: "Wish");

            migrationBuilder.DropColumn(
                name: "WishCategoryId",
                table: "Wish");
        }
    }
}
