using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EKE.Data.Migrations
{
    public partial class AttributeTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Length",
                table: "Vt_Trip",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<decimal>(
                name: "Time",
                table: "Vt_Trip",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Vt_Trip");

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                table: "Vt_Trip",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
