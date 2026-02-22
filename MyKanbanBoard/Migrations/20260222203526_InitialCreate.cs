using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyKanbanBoard.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SprintId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    IsExpanded = table.Column<bool>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStories_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserStoryId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    TotalSeconds = table.Column<long>(nullable: false),
                    ActiveSinceUtc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_UserStories_UserStoryId",
                        column: x => x.UserStoryId,
                        principalTable: "UserStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserStoryId",
                table: "Tasks",
                column: "UserStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_SprintId",
                table: "UserStories",
                column: "SprintId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "UserStories");

            migrationBuilder.DropTable(
                name: "Sprints");
        }
    }
}
