using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssReader.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedFolderOwnerFkIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folder_User_UserId",
                table: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Folder_UserId",
                table: "Folder");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Folder");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_OwnerId",
                table: "Folder",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folder_User_OwnerId",
                table: "Folder",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folder_User_OwnerId",
                table: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Folder_OwnerId",
                table: "Folder");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Folder",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Folder_UserId",
                table: "Folder",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folder_User_UserId",
                table: "Folder",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
