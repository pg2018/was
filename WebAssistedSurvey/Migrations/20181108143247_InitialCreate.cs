using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAssistedSurvey.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    IsMultidays = table.Column<bool>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Summery = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyEvents", x => x.EventID);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    SurveyID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    ContactName = table.Column<string>(nullable: true),
                    WantNewsletter = table.Column<bool>(nullable: false),
                    ContactEmail = table.Column<string>(nullable: true),
                    GoodGuy = table.Column<string>(nullable: true),
                    BadGuy = table.Column<string>(nullable: true),
                    Feedback = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.SurveyID);
                    table.ForeignKey(
                        name: "FK_Surveys_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_EventID",
                table: "Surveys",
                column: "EventID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
