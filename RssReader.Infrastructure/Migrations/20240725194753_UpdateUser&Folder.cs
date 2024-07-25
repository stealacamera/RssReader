using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssReader.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Tag",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Folder",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Folder",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name_OwnerId",
                table: "Tag",
                columns: new[] { "Name", "OwnerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_OwnerId",
                table: "Tag",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_Name_OwnerId",
                table: "Folder",
                columns: new[] { "Name", "OwnerId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_User_OwnerId",
                table: "Tag",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_User_OwnerId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_Name_OwnerId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_OwnerId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Folder_Name_OwnerId",
                table: "Folder");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Folder");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Folder",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
