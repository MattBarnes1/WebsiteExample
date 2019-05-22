using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication5.Data.Migrations
{
    public partial class Migration1125 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModelsMade",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "myLore",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoreIdeaStatus = table.Column<int>(nullable: false),
                    LoreBody = table.Column<string>(nullable: true),
                    LoreName = table.Column<string>(nullable: false),
                    ProposerUserId = table.Column<string>(nullable: false),
                    LoreID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myLore", x => x.ID);
                    table.ForeignKey(
                        name: "FK_myLore_myLore_LoreID",
                        column: x => x.LoreID,
                        principalTable: "myLore",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_myLore_LoreID",
                table: "myLore",
                column: "LoreID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "myLore");

            migrationBuilder.DropColumn(
                name: "ModelsMade",
                table: "AspNetUsers");
        }
    }
}
