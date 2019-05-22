using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication5.Data.Migrations
{
    public partial class EventAndTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "myGlobalEvents",
                columns: table => new
                {
                    myEventString = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myGlobalEvents", x => x.myEventString);
                });

            migrationBuilder.CreateTable(
                name: "myTasks",
                columns: table => new
                {
                    TaskDescription = table.Column<string>(nullable: false),
                    ClaimedByUserID = table.Column<int>(nullable: false),
                    TaskStart = table.Column<DateTime>(nullable: false),
                    TaskEndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myTasks", x => x.TaskDescription);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "myGlobalEvents");

            migrationBuilder.DropTable(
                name: "myTasks");
        }
    }
}
