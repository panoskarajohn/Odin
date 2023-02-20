using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Slip.Service.Migrations
{
    /// <inheritdoc />
    public partial class betstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BetStatus",
                table: "Bets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetStatus",
                table: "Bets");
        }
    }
}
