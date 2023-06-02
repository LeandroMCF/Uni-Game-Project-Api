using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uni.Project.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class download : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Download",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Download",
                table: "Users");
        }
    }
}
