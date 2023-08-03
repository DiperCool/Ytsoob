using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ytsoob.Services.Posts.Migrations
{
    /// <inheritdoc />
    public partial class YtsooberAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "ytsoobers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "ytsoobers");
        }
    }
}
