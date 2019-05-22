using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication5.Data.Migrations
{
    public partial class ExpandedUserSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_myTasks_AspNetUsers_UserDataId",
                table: "myTasks");

            migrationBuilder.DropIndex(
                name: "IX_myTasks_UserDataId",
                table: "myTasks");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "myTasks");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimedByID",
                table: "myTasks",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "BackgroundUrl",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "myRequests",
                columns: table => new
                {
                    IDKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestersID = table.Column<string>(nullable: false),
                    RequestBody = table.Column<string>(nullable: false),
                    myType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myRequests", x => x.IDKey);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "myRequests");

            migrationBuilder.DropColumn(
                name: "BackgroundUrl",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimedByID",
                table: "myTasks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserDataId",
                table: "myTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_myTasks_UserDataId",
                table: "myTasks",
                column: "UserDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_myTasks_AspNetUsers_UserDataId",
                table: "myTasks",
                column: "UserDataId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
