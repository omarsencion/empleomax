using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpleosWebMax.Infrastructure.Core.Migrations
{
    public partial class plans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDefaultPlan = table.Column<bool>(type: "bit", nullable: false),
                    JobVacancyNumber = table.Column<int>(type: "int", nullable: false),
                    PriorityJobNumber = table.Column<int>(type: "int", nullable: false),
                    CanditateMessageNumber = table.Column<int>(type: "int", nullable: false),
                    IsCanditateSearch = table.Column<bool>(type: "bit", nullable: false),
                    PriceOfJobVacancyBonus = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
