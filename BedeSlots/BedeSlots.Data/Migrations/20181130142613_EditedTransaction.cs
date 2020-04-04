using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.Data.Migrations
{
    public partial class EditedTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "GameType",
                table: "Transactions",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "CvvNumber",
                table: "BankCards",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Transactions",
                newName: "GameType");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CvvNumber",
                table: "BankCards",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
