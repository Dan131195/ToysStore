using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringOrdineProdotti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordini_Prodotti_ProdottoGiocattoloId",
                table: "Ordini");

            migrationBuilder.DropIndex(
                name: "IX_Ordini_ProdottoGiocattoloId",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "ProdottoGiocattoloId",
                table: "Ordini");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProdottoGiocattoloId",
                table: "Ordini",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_ProdottoGiocattoloId",
                table: "Ordini",
                column: "ProdottoGiocattoloId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordini_Prodotti_ProdottoGiocattoloId",
                table: "Ordini",
                column: "ProdottoGiocattoloId",
                principalTable: "Prodotti",
                principalColumn: "GiocattoloId");
        }
    }
}
