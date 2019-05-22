using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication5.Data.Migrations
{
    public partial class UpdatedUserDataToHaveCurrentItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TasksCompleted",
                table: "AspNetUsers",
                newName: "ClaimedTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClaimedTasks",
                table: "AspNetUsers",
                newName: "TasksCompleted");
        }
    }
}
