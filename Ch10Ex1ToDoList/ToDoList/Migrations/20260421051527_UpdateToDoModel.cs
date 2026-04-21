using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ch10ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToDoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Statuses_StatusId",
                table: "ToDos");

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "ToDos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Statuses_StatusId",
                table: "ToDos",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Statuses_StatusId",
                table: "ToDos");

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "ToDos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Statuses_StatusId",
                table: "ToDos",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "StatusId");
        }
    }
}
