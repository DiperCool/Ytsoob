using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ytsoob.Services.Posts.Migrations
{
    /// <inheritdoc />
    public partial class PostsReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "reaction_stats_id",
                schema: "posts",
                table: "posts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "reaction_stats",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    like = table.Column<long>(type: "bigint", nullable: false),
                    dislike = table.Column<long>(type: "bigint", nullable: false),
                    angry = table.Column<long>(type: "bigint", nullable: false),
                    wonder = table.Column<long>(type: "bigint", nullable: false),
                    crying = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reaction_stats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reactions",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    reactiontype = table.Column<int>(name: "reaction_type", type: "integer", nullable: false),
                    entityid = table.Column<string>(name: "entity_id", type: "text", nullable: false),
                    entitytype = table.Column<string>(name: "entity_type", type: "text", nullable: false),
                    ytsooberid = table.Column<long>(name: "ytsoober_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_reactions_ytsoobers_ytsoober_id",
                        column: x => x.ytsooberid,
                        principalTable: "ytsoobers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_posts_reaction_stats_id",
                schema: "posts",
                table: "posts",
                column: "reaction_stats_id");

            migrationBuilder.CreateIndex(
                name: "ix_reaction_stats_id",
                schema: "posts",
                table: "reaction_stats",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reactions_id",
                schema: "posts",
                table: "reactions",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reactions_ytsoober_id",
                schema: "posts",
                table: "reactions",
                column: "ytsoober_id");

            migrationBuilder.AddForeignKey(
                name: "fk_posts_reaction_stats_reaction_stats_id",
                schema: "posts",
                table: "posts",
                column: "reaction_stats_id",
                principalSchema: "posts",
                principalTable: "reaction_stats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_posts_reaction_stats_reaction_stats_id",
                schema: "posts",
                table: "posts");

            migrationBuilder.DropTable(
                name: "reaction_stats",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "reactions",
                schema: "posts");

            migrationBuilder.DropIndex(
                name: "ix_posts_reaction_stats_id",
                schema: "posts",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "reaction_stats_id",
                schema: "posts",
                table: "posts");
        }
    }
}
