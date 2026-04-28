using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnrichSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DailyRoutineRecordAddColumnAndUpdateType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DailyRoutineId",
                table: "DailyRoutineRecords",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "DailyRoutineRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "DailyRoutineRecords");

            migrationBuilder.AlterColumn<string>(
                name: "DailyRoutineId",
                table: "DailyRoutineRecords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
