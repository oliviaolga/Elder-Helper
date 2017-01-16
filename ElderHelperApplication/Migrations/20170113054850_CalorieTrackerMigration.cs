using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElderHelperApplication.Migrations
{
    public partial class CalorieTrackerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    GoalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    berat = table.Column<double>(nullable: false),
                    calorieGoal = table.Column<double>(nullable: false),
                    calorieToday = table.Column<double>(nullable: false),
                    dataToday = table.Column<double>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    gender = table.Column<string>(nullable: true),
                    nama = table.Column<string>(nullable: true),
                    tinggi = table.Column<double>(nullable: false),
                    umur = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.GoalId);
                });

            migrationBuilder.CreateTable(
                name: "Calculations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GoalId = table.Column<int>(nullable: false),
                    amount = table.Column<double>(nullable: false),
                    date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calculations_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "GoalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalCalories",
                columns: table => new
                {
                    GuId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CalculationId = table.Column<int>(nullable: true),
                    GoalId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    calorieToday = table.Column<double>(nullable: false),
                    date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalCalories", x => x.GuId);
                    table.ForeignKey(
                        name: "FK_TotalCalories_Calculations_CalculationId",
                        column: x => x.CalculationId,
                        principalTable: "Calculations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TotalCalories_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "GoalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calculations_GoalId",
                table: "Calculations",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalCalories_CalculationId",
                table: "TotalCalories",
                column: "CalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalCalories_GoalId",
                table: "TotalCalories",
                column: "GoalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TotalCalories");

            migrationBuilder.DropTable(
                name: "Calculations");

            migrationBuilder.DropTable(
                name: "Goals");
        }
    }
}
