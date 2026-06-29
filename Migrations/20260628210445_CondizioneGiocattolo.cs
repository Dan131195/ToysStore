using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class CondizioneGiocattolo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condizioni",
                table: "Prodotti");

            migrationBuilder.AddColumn<int>(
                name: "CondizioneId",
                table: "Prodotti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Condizione",
                columns: table => new
                {
                    CondizioneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCondizione = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescrizioneCondizione = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condizione", x => x.CondizioneId);
                });

            migrationBuilder.InsertData(
                table: "Condizione",
                columns: new[] { "CondizioneId", "DescrizioneCondizione", "NomeCondizione" },
                values: new object[,]
                {
                    { 1, "Il giocattolo è nuovo, mai aperto e si trova nella sua confezione originale con i sigilli intatti. Non presenta alcun danno.", "Nuovo e sigillato" },
                    { 2, "Il giocattolo non è mai stato usato per giocare, ma non ha più la scatola originale o le etichette. Non presenta il minimo segno di usura ed è completo di tutti gli accessori.", "Nuovo senza confezione" },
                    { 3, "Usato pochissimo e tenuto con cura. Non ci sono difetti visibili, graffi o scoloriture. Tutti i pezzi originali sono presenti e, se elettronico, funziona perfettamente.", "Ottimo" },
                    { 4, "Il giocattolo è stato usato e amato. Mostra segni di usura normali e leggeri (es. piccoli graffi superficiali o adesivi leggermente consumati). È comunque completo per poterci giocare e strutturalmente integro.", "Buono" },
                    { 5, "Mostra segni di usura evidenti dovuti a un uso frequente. Potrebbe avere graffi profondi, vernice scolorita o mancare di accessori non essenziali (che non impediscono il funzionamento principale del gioco).", "Accettabile" },
                    { 6, "Il giocattolo presenta difetti importanti, componenti elettroniche non funzionanti, rotture o pezzi mancanti fondamentali. Viene venduto principalmente per essere riparato, riutilizzato per parti di ricambio (es. lotti di mattoncini Lego) o restauro.", "Con difetti" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prodotti_CondizioneId",
                table: "Prodotti",
                column: "CondizioneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prodotti_Condizione_CondizioneId",
                table: "Prodotti",
                column: "CondizioneId",
                principalTable: "Condizione",
                principalColumn: "CondizioneId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prodotti_Condizione_CondizioneId",
                table: "Prodotti");

            migrationBuilder.DropTable(
                name: "Condizione");

            migrationBuilder.DropIndex(
                name: "IX_Prodotti_CondizioneId",
                table: "Prodotti");

            migrationBuilder.DropColumn(
                name: "CondizioneId",
                table: "Prodotti");

            migrationBuilder.AddColumn<string>(
                name: "Condizioni",
                table: "Prodotti",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
