using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ytsoob.Services.Posts.Migrations
{
    /// <inheritdoc />
    public partial class Polls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "polls",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    postid = table.Column<long>(name: "post_id", type: "bigint", nullable: false),
                    pollanswertype = table.Column<string>(name: "poll_answer_type", type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_polls", x => x.id);
                    table.ForeignKey(
                        name: "fk_polls_posts_post_id",
                        column: x => x.postid,
                        principalSchema: "posts",
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "options",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    optiontitle = table.Column<string>(name: "option_title", type: "character varying(50)", maxLength: 50, nullable: false),
                    optioncount = table.Column<long>(name: "option_count", type: "bigint", nullable: false),
                    fiction = table.Column<long>(type: "bigint", nullable: false),
                    pollid = table.Column<long>(name: "poll_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_options", x => x.id);
                    table.ForeignKey(
                        name: "fk_options_polls_poll_id",
                        column: x => x.pollid,
                        principalSchema: "posts",
                        principalTable: "polls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "voters",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    ytsooberid = table.Column<long>(name: "ytsoober_id", type: "bigint", nullable: false),
                    optionid = table.Column<long>(name: "option_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_voters", x => x.id);
                    table.ForeignKey(
                        name: "fk_voters_options_option_id",
                        column: x => x.optionid,
                        principalSchema: "posts",
                        principalTable: "options",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_voters_ytsoobers_ytsoober_id",
                        column: x => x.ytsooberid,
                        principalTable: "ytsoobers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_options_id",
                schema: "posts",
                table: "options",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_options_poll_id",
                schema: "posts",
                table: "options",
                column: "poll_id");

            migrationBuilder.CreateIndex(
                name: "ix_polls_id",
                schema: "posts",
                table: "polls",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_polls_post_id",
                schema: "posts",
                table: "polls",
                column: "post_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_voters_id",
                schema: "posts",
                table: "voters",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_voters_option_id",
                schema: "posts",
                table: "voters",
                column: "option_id");

            migrationBuilder.CreateIndex(
                name: "ix_voters_ytsoober_id",
                schema: "posts",
                table: "voters",
                column: "ytsoober_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "voters",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "options",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "polls",
                schema: "posts");
        }
    }
}
