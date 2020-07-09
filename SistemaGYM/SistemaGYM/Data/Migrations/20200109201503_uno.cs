using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaGYM.Data.Migrations
{
    public partial class uno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "Membresia");

            migrationBuilder.AddColumn<int>(
                name: "ExpiraEn",
                table: "Membresia",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiraEn",
                table: "Membresia");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishDate",
                table: "Membresia",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
