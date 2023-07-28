using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ytsoob.Services.Posts.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "posts");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "posts",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_posts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ytsoobers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identityid = table.Column<Guid>(name: "identity_id", type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ytsoobers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contents",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    contenttext = table.Column<string>(name: "content_text", type: "character varying(50)", maxLength: 50, nullable: false),
                    postid = table.Column<long>(name: "post_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contents", x => x.id);
                    table.ForeignKey(
                        name: "fk_contents_posts_post_id",
                        column: x => x.postid,
                        principalSchema: "posts",
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contents_id",
                schema: "posts",
                table: "contents",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contents_post_id",
                schema: "posts",
                table: "contents",
                column: "post_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_posts_id",
                schema: "posts",
                table: "posts",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contents",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "ytsoobers");

            migrationBuilder.DropTable(
                name: "posts",
                schema: "posts");
        }
    }
}
