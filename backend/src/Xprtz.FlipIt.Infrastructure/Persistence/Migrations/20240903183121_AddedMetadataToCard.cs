using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xprtz.FlipIt.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedMetadataToCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewedAt",
                table: "Cards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCorrectAnswers",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfViews",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastViewedAt",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "NumberOfCorrectAnswers",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "NumberOfViews",
                table: "Cards");
        }
    }
}
