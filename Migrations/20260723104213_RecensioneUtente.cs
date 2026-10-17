using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class RecensioneUtente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecensioniProdotto");

            migrationBuilder.CreateTable(
                name: "RecensioniUtente",
                columns: table => new
                {
                    RecensioneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecensioneTesto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Valutazione = table.Column<int>(type: "int", nullable: false),
                    DataRecensione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcquirenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenditoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrdineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecensioniUtente", x => x.RecensioneId);
                    table.ForeignKey(
                        name: "FK_RecensioniUtente_Ordini_OrdineId",
                        column: x => x.OrdineId,
                        principalTable: "Ordini",
                        principalColumn: "OrdineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecensioniUtente_Utenti_AcquirenteId",
                        column: x => x.AcquirenteId,
                        principalTable: "Utenti",
                        principalColumn: "UtenteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecensioniUtente_Utenti_VenditoreId",
                        column: x => x.VenditoreId,
                        principalTable: "Utenti",
                        principalColumn: "UtenteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecensioniUtente_AcquirenteId",
                table: "RecensioniUtente",
                column: "AcquirenteId");

            migrationBuilder.CreateIndex(
                name: "IX_RecensioniUtente_OrdineId",
                table: "RecensioniUtente",
                column: "OrdineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecensioniUtente_VenditoreId",
                table: "RecensioniUtente",
                column: "VenditoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecensioniUtente");

            migrationBuilder.CreateTable(
                name: "RecensioniProdotto",
                columns: table => new
                {
                    RecensioneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataRecensione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecensioneTesto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Valutazione = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecensioniProdotto", x => x.RecensioneId);
                    table.ForeignKey(
                        name: "FK_RecensioniProdotto_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecensioniProdotto_Prodotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "Prodotti",
                        principalColumn: "GiocattoloId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecensioniProdotto_ProdottoId",
                table: "RecensioniProdotto",
                column: "ProdottoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecensioniProdotto_UserId",
                table: "RecensioniProdotto",
                column: "UserId");
        }
    }
}
