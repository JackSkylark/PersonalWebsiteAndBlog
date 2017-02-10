using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JohnSlaughter.Data.BlogData.Migrations
{
    public partial class PostFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Post",
                newName: "FileName");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Post",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Post_FileName",
                table: "Post",
                column: "FileName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Post_FileName",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Post",
                newName: "Path");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Post",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
