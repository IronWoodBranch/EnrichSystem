using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnrichSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestUPdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Quests");
        }
    }
}
