using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ytsoob.Services.Payment.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "ytsoobers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identityid = table.Column<Guid>(name: "identity_id", type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    stripeid = table.Column<string>(name: "stripe_id", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ytsoobers", x => x.id);
                });

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
                    productid = table.Column<string>(name: "product_id", type: "text", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "price_product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    pricepaymentid = table.Column<string>(name: "price_payment_id", type: "text", nullable: false),
                    recurringdays = table.Column<int>(name: "recurring_days", type: "integer", nullable: false),
                    subscriptionid = table.Column<long>(name: "subscription_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_product", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_product_subscriptions_subscription_id",
                        column: x => x.subscriptionid,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_price_product_subscription_id",
                table: "price_product",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscriptions_ytsoober_id",
                table: "subscriptions",
                column: "ytsoober_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "price_product");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "ytsoobers");
        }
    }
}
