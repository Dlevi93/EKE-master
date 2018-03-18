using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EKE.Data.Migrations
{
    public partial class VtInitital2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VtUserId",
                table: "Vt_Trip",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VtYearId",
                table: "Vt_Trip",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vt_Trip_VtUserId",
                table: "Vt_Trip",
                column: "VtUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vt_Trip_VtYearId",
                table: "Vt_Trip",
                column: "VtYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vt_Trip_Vt_User_VtUserId",
                table: "Vt_Trip",
                column: "VtUserId",
                principalTable: "Vt_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vt_Trip_Vt_Year_VtYearId",
                table: "Vt_Trip",
                column: "VtYearId",
                principalTable: "Vt_Year",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vt_Trip_Vt_User_VtUserId",
                table: "Vt_Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_Vt_Trip_Vt_Year_VtYearId",
                table: "Vt_Trip");

            migrationBuilder.DropIndex(
                name: "IX_Vt_Trip_VtUserId",
                table: "Vt_Trip");

            migrationBuilder.DropIndex(
                name: "IX_Vt_Trip_VtYearId",
                table: "Vt_Trip");

            migrationBuilder.DropColumn(
                name: "VtUserId",
                table: "Vt_Trip");

            migrationBuilder.DropColumn(
                name: "VtYearId",
                table: "Vt_Trip");
        }
    }
}
