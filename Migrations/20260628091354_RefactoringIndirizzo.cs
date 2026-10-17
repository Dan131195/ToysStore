using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringIndirizzo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteId",
                table: "Ordini");

            migrationBuilder.DropIndex(
                name: "IX_Ordini_IndirizzoUtenteId",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "IndirizzoUtenteId",
                table: "Ordini");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "IndirizzoUtenteId",
                table: "Ordini",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_IndirizzoUtenteId",
                table: "Ordini",
                column: "IndirizzoUtenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteId",
                table: "Ordini",
                column: "IndirizzoUtenteId",
                principalTable: "IndirizziUtenti",
                principalColumn: "IndirizzoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
