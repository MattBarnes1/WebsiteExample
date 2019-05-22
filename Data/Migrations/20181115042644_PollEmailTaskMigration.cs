using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication5.Data.Migrations
{
    public partial class PollEmailTaskMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimedByUserID",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "TaskEndDate",
                table: "myTasks");

            migrationBuilder.RenameColumn(
                name: "TaskStart",
                table: "myTasks",
                newName: "TaskStartDate");

            migrationBuilder.AddColumn<string>(
                name: "ClaimedByID",
                table: "myTasks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "MarkedCompleted",
                table: "myTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserDataId",
                table: "myTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "myState",
                table: "myTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxRequestableTasks",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PollID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompleted",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DevBlogTemplate",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevBlogTemplate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "myEmails",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserIDFrom = table.Column<string>(nullable: false),
                    UserIDTo = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    ReadByRecipiant = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myEmails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "myPolls",
                columns: table => new
                {
                    PollID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PollQuestion = table.Column<string>(nullable: false),
                    Choices = table.Column<string>(nullable: false),
                    VotesPerChoice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myPolls", x => x.PollID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_myTasks_UserDataId",
                table: "myTasks",
                column: "UserDataId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PollID",
                table: "AspNetUsers",
                column: "PollID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_myPolls_PollID",
                table: "AspNetUsers",
                column: "PollID",
                principalTable: "myPolls",
                principalColumn: "PollID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_myTasks_AspNetUsers_UserDataId",
                table: "myTasks",
                column: "UserDataId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_myPolls_PollID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_myTasks_AspNetUsers_UserDataId",
                table: "myTasks");

            migrationBuilder.DropTable(
                name: "DevBlogTemplate");

            migrationBuilder.DropTable(
                name: "myEmails");

            migrationBuilder.DropTable(
                name: "myPolls");

            migrationBuilder.DropIndex(
                name: "IX_myTasks_UserDataId",
                table: "myTasks");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PollID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClaimedByID",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "MarkedCompleted",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "myState",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxRequestableTasks",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PollID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TasksCompleted",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TaskStartDate",
                table: "myTasks",
                newName: "TaskStart");

            migrationBuilder.AddColumn<int>(
                name: "ClaimedByUserID",
                table: "myTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaskEndDate",
                table: "myTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
