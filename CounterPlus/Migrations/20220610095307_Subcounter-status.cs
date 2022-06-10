using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CounterPlus.Migrations
{
    public partial class Subcounterstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_AspNetUsers_OwnerId",
                table: "Counters");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Counters",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Counters_OwnerId",
                table: "Counters",
                newName: "IX_Counters_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "SubCounters",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_AspNetUsers_UserId",
                table: "Counters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counters_AspNetUsers_UserId",
                table: "Counters");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "SubCounters");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Counters",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Counters_UserId",
                table: "Counters",
                newName: "IX_Counters_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counters_AspNetUsers_OwnerId",
                table: "Counters",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
