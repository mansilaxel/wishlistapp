using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace wishListBackend.Migrations
{
    public partial class qsdfsq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Wish",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wish_UserId",
                table: "Wish",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wish_User_UserId",
                table: "Wish",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wish_User_UserId",
                table: "Wish");

            migrationBuilder.DropIndex(
                name: "IX_Wish_UserId",
                table: "Wish");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Wish");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");
        }
    }
}
