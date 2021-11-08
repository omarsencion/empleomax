using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpleosWebMax.Infrastructure.Core.Migrations
{
    public partial class taxreceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxReceipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceFrom = table.Column<long>(type: "bigint", nullable: false),
                    SequenceTo = table.Column<long>(type: "bigint", nullable: false),
                    SequenceActual = table.Column<long>(type: "bigint", nullable: false),
                    IsCompany = table.Column<bool>(type: "bit", nullable: false),
                    IsCanditate = table.Column<bool>(type: "bit", nullable: false),
                    IsInternationalCompany = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxReceipts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxReceipts");
        }
    }
}
