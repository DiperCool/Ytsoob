using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ytsoob.Services.Posts.Migrations
{
    /// <inheritdoc />
    public partial class Comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comments",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    commentcontent = table.Column<string>(name: "comment_content", type: "text", nullable: false),
                    reactionstatsid = table.Column<long>(name: "reaction_stats_id", type: "bigint", nullable: false),
                    files = table.Column<List<string>>(type: "text[]", nullable: false),
                    postid = table.Column<long>(name: "post_id", type: "bigint", nullable: false),
                    discriminator = table.Column<string>(type: "text", nullable: false),
                    commentid = table.Column<long>(name: "comment_id", type: "bigint", nullable: true),
                    repliedtocommentid = table.Column<long>(name: "replied_to_comment_id", type: "bigint", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_base_comments_base_comments_comment_id",
                        column: x => x.commentid,
                        principalSchema: "posts",
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_base_comments_base_comments_replied_to_comment_id",
                        column: x => x.repliedtocommentid,
                        principalSchema: "posts",
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comments_reaction_stats_reaction_stats_id",
                        column: x => x.reactionstatsid,
                        principalSchema: "posts",
                        principalTable: "reaction_stats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_base_comments_comment_id",
                schema: "posts",
                table: "comments",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "ix_base_comments_replied_to_comment_id",
                schema: "posts",
                table: "comments",
                column: "replied_to_comment_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_id",
                schema: "posts",
                table: "comments",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_comments_reaction_stats_id",
                schema: "posts",
                table: "comments",
                column: "reaction_stats_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments",
                schema: "posts");
        }
    }
}
