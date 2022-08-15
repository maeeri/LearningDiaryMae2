using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearningDiaryMae2.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaryTopic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EstimatedTimeToMaster = table.Column<double>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    StartLearningDate = table.Column<DateTime>(nullable: false),
                    InProgress = table.Column<bool>(nullable: false),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    LastEditDate = table.Column<DateTime>(nullable: true),
                    TimeSpent = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryTopic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiaryTask",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Deadline = table.Column<DateTime>(nullable: true),
                    Priority = table.Column<string>(nullable: true),
                    Done = table.Column<bool>(nullable: false),
                    DiaryTopic = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryTask_DiaryTopic_DiaryTopic",
                        column: x => x.DiaryTopic,
                        principalTable: "DiaryTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaryTask_DiaryTopic",
                table: "DiaryTask",
                column: "DiaryTopic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaryTask");

            migrationBuilder.DropTable(
                name: "DiaryTopic");
        }
    }
}
