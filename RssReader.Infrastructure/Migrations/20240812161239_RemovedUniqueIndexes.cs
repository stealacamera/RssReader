using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssReader.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tag_Name_OwnerId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Folder_Name_OwnerId",
                table: "Folder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name_OwnerId",
                table: "Tag",
                columns: new[] { "Name", "OwnerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Folder_Name_OwnerId",
                table: "Folder",
                columns: new[] { "Name", "OwnerId" },
                unique: true);
        }
    }
}
