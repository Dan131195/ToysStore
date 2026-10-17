using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringIndirizzo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteIndirizzoId",
                table: "Ordini");

            migrationBuilder.DropIndex(
                name: "IX_Ordini_IndirizzoUtenteIndirizzoId",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "IndirizzoUtenteIndirizzoId",
                table: "Ordini");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IndirizzoUtenteIndirizzoId",
                table: "Ordini",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_IndirizzoUtenteIndirizzoId",
                table: "Ordini",
                column: "IndirizzoUtenteIndirizzoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteIndirizzoId",
                table: "Ordini",
                column: "IndirizzoUtenteIndirizzoId",
                principalTable: "IndirizziUtenti",
                principalColumn: "IndirizzoId");
        }
    }
}
