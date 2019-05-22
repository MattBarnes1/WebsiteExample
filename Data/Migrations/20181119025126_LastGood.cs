using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication5.Data.Migrations
{
    public partial class LastGood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_myPolls_PollID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DevBlogTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_myTasks",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "VotesPerChoice",
                table: "myPolls");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Choices",
                table: "myPolls",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PollID",
                table: "myPolls",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "PollID",
                table: "AspNetUsers",
                newName: "PollChoicePollID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_PollID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_PollChoicePollID");

            migrationBuilder.AlterColumn<string>(
                name: "TaskDescription",
                table: "myTasks",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "TaskID",
                table: "myTasks",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "ShortTaskDescription",
                table: "myTasks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TaskEnd",
                table: "myTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isFinished",
                table: "myPolls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EmailRepliedTo",
                table: "myEmails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeSent",
                table: "myEmails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<int>(
                name: "TasksCompleted",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxRequestableTasks",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeDirectory",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Joined",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PublicName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "PK_myTasks",
                table: "myTasks",
                column: "TaskID");

            migrationBuilder.CreateTable(
                name: "myBlogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlogFile = table.Column<string>(nullable: true),
                    CalculateBlogHash = table.Column<string>(nullable: true),
                    Approver = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myBlogs", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "myScreenShots",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlogFile = table.Column<string>(nullable: true),
                    CalculateBlogHash = table.Column<string>(nullable: true),
                    ApproverID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myScreenShots", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PollChoice",
                columns: table => new
                {
                    PollID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    mySelectedChoice = table.Column<string>(nullable: false),
                    PollID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollChoice", x => x.PollID);
                    table.ForeignKey(
                        name: "FK_PollChoice_myPolls_PollID1",
                        column: x => x.PollID1,
                        principalTable: "myPolls",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollChoice_PollID1",
                table: "PollChoice",
                column: "PollID1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PollChoice_PollChoicePollID",
                table: "AspNetUsers",
                column: "PollChoicePollID",
                principalTable: "PollChoice",
                principalColumn: "PollID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PollChoice_PollChoicePollID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "myBlogs");

            migrationBuilder.DropTable(
                name: "myScreenShots");

            migrationBuilder.DropTable(
                name: "PollChoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_myTasks",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "TaskID",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "ShortTaskDescription",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "TaskEnd",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "isFinished",
                table: "myPolls");

            migrationBuilder.DropColumn(
                name: "EmailRepliedTo",
                table: "myEmails");

            migrationBuilder.DropColumn(
                name: "TimeSent",
                table: "myEmails");

            migrationBuilder.DropColumn(
                name: "HomeDirectory",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Joined",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "myPolls",
                newName: "Choices");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "myPolls",
                newName: "PollID");

            migrationBuilder.RenameColumn(
                name: "PollChoicePollID",
                table: "AspNetUsers",
                newName: "PollID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_PollChoicePollID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_PollID");

            migrationBuilder.AlterColumn<string>(
                name: "TaskDescription",
                table: "myTasks",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "VotesPerChoice",
                table: "myPolls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "TasksCompleted",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MaxRequestableTasks",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_myTasks",
                table: "myTasks",
                column: "TaskDescription");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_myPolls_PollID",
                table: "AspNetUsers",
                column: "PollID",
                principalTable: "myPolls",
                principalColumn: "PollID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
