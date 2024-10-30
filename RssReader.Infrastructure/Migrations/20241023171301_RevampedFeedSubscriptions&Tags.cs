using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RssReader.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RevampedFeedSubscriptionsTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedSubscription",
                table: "FeedSubscription");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FeedSubscription",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedSubscription",
                table: "FeedSubscription",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FeedSubscriptionTag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false),
                    FeedSubscriptionId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSubscriptionTag", x => new { x.TagId, x.FeedSubscriptionId });
                    table.ForeignKey(
                        name: "FK_FeedSubscriptionTag_FeedSubscription_FeedSubscriptionId",
                        column: x => x.FeedSubscriptionId,
                        principalTable: "FeedSubscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedSubscriptionTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedSubscription_FeedId_FolderId",
                table: "FeedSubscription",
                columns: new[] { "FeedId", "FolderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedSubscriptionTag_FeedSubscriptionId",
                table: "FeedSubscriptionTag",
                column: "FeedSubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedSubscriptionTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedSubscription",
                table: "FeedSubscription");

            migrationBuilder.DropIndex(
                name: "IX_FeedSubscription_FeedId_FolderId",
                table: "FeedSubscription");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FeedSubscription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedSubscription",
                table: "FeedSubscription",
                columns: new[] { "FeedId", "FolderId" });

            migrationBuilder.CreateTable(
                name: "FeedTag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false),
                    FeedId = table.Column<int>(type: "integer", nullable: false),
                    FolderId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedTag", x => new { x.TagId, x.FeedId, x.FolderId });
                    table.ForeignKey(
                        name: "FK_FeedTag_FeedSubscription_FeedId_FolderId",
                        columns: x => new { x.FeedId, x.FolderId },
                        principalTable: "FeedSubscription",
                        principalColumns: new[] { "FeedId", "FolderId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedTag_FeedId_FolderId",
                table: "FeedTag",
                columns: new[] { "FeedId", "FolderId" });
        }
    }
}
