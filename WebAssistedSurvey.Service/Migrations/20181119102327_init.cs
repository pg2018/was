using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAssistedSurvey.Service.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebEvents",
                columns: table => new
                {
                    WebEventID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    IsMultidays = table.Column<bool>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Summery = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebEvents", x => x.WebEventID);
                });

            migrationBuilder.CreateTable(
                name: "WebSurveys",
                columns: table => new
                {
                    WebSurveyID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebEventID = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_WebSurveys", x => x.WebSurveyID);
                    table.ForeignKey(
                        name: "FK_WebSurveys_WebEvents_WebEventID",
                        column: x => x.WebEventID,
                        principalTable: "WebEvents",
                        principalColumn: "WebEventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebSurveys_WebEventID",
                table: "WebSurveys",
                column: "WebEventID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebSurveys");

            migrationBuilder.DropTable(
                name: "WebEvents");
        }
    }
}
