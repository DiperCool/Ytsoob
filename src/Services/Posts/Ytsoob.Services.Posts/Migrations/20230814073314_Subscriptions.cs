using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ytsoob.Services.Posts.Migrations
{
    /// <inheritdoc />
    public partial class Subscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "subscription_id",
                schema: "posts",
                table: "posts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    photo = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    ytsooberid = table.Column<long>(name: "ytsoober_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "fk_subscriptions_ytsoobers_ytsoober_id",
                        column: x => x.ytsooberid,
                        principalTable: "ytsoobers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_posts_subscription_id",
                schema: "posts",
                table: "posts",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscriptions_ytsoober_id",
                table: "subscriptions",
                column: "ytsoober_id");

            migrationBuilder.AddForeignKey(
                name: "fk_posts_subscriptions_subscription_id",
                schema: "posts",
                table: "posts",
                column: "subscription_id",
                principalTable: "subscriptions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_posts_subscriptions_subscription_id",
                schema: "posts",
                table: "posts");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropIndex(
                name: "ix_posts_subscription_id",
                schema: "posts",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "subscription_id",
                schema: "posts",
                table: "posts");
        }
    }
}
