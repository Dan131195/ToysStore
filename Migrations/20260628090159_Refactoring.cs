using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class Refactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordini_AspNetUsers_UserId",
                table: "Ordini");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteId",
                table: "Ordini");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordini_AspNetUsers_UserId",
                table: "Ordini",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteId",
                table: "Ordini",
                column: "IndirizzoUtenteId",
                principalTable: "IndirizziUtenti",
                principalColumn: "IndirizzoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordini_AspNetUsers_UserId",
                table: "Ordini");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteId",
                table: "Ordini");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordini_AspNetUsers_UserId",
                table: "Ordini",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteId",
                table: "Ordini",
                column: "IndirizzoUtenteId",
                principalTable: "IndirizziUtenti",
                principalColumn: "IndirizzoId");
        }
    }
}
