using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    public partial class video : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "videos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_videos_CategoryId",
                table: "videos",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_videos_Categories_CategoryId",
                table: "videos",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_videos_Categories_CategoryId",
                table: "videos");

            migrationBuilder.DropIndex(
                name: "IX_videos_CategoryId",
                table: "videos");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "videos");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "videos",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
