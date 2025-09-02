using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xprtz.FlipIt.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedLabelsToTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackLabel",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FrontLabel",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackLabel",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "FrontLabel",
                table: "Topics");
        }
    }
}
