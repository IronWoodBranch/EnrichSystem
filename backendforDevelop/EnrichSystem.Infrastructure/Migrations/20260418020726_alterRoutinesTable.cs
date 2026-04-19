using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnrichSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterRoutinesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Ledgers",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "CompleteReward",
                table: "DailyRoutines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FailedPunish",
                table: "DailyRoutines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteReward",
                table: "DailyRoutines");

            migrationBuilder.DropColumn(
                name: "FailedPunish",
                table: "DailyRoutines");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Ledgers",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
