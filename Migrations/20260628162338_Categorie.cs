using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class Categorie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Prodotti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categorie",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCategoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescrizioneCategoria = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorie", x => x.CategoriaId);
                });

            migrationBuilder.InsertData(
                table: "Categorie",
                columns: new[] { "CategoriaId", "DescrizioneCategoria", "NomeCategoria" },
                values: new object[,]
                {
                    { 1, "Sonagli, tappeti gioco, giostrine, primi passi, cavalcabili, massaggiagengive.", "Prima Infanzia (0-36 mesi)" },
                    { 2, "LEGO, Mega Bloks, Playmobil, costruzioni in legno, set magnetici.", "Costruzioni e Mattoncini" },
                    { 3, "Supereroi, personaggi di film/serie TV, anime, wrestling, dinosauri, Gormiti.", "Action Figure e Personaggi" },
                    { 4, "Barbie, bambolotti (es. Cicciobello), case delle bambole, vestiti, passeggini giocattolo.", "Bambole e Accessori" },
                    { 5, "Orsacchiotti, personaggi animati in pezza, animali di peluche, doudou.", "Peluche e Pupazzi" },
                    { 6, "Giochi in scatola classici, giochi di carte, scacchi, dama.", "Giochi da Tavolo e di Società" },
                    { 7, "Puzzle tradizionali, puzzle 3D, cubi di Rubik, puzzle in legno.", "Puzzle e Rompicapo" },
                    { 8, "Macchinine (es. Hot Wheels), trenini, piste elettriche, droni, barche, aerei.", "Veicoli, Radiocomandati e Piste" },
                    { 9, "Microscopi, kit per esperimenti (STEM), mappamondi, tablet educativi, giochi di logica.", "Educativi e Scientifici" },
                    { 10, "Pasta da modellare (Play-Doh), kit per braccialetti, colori, lavagne, timbri.", "Creatività, Arti e Mestieri" },
                    { 11, "Cucine giocattolo, finti attrezzi da lavoro, registratori di cassa, set da dottore.", "Giochi d'Imitazione e Ruolo" },
                    { 12, "Vestiti di Carnevale/Halloween, maschere, bacchette magiche, accessori.", "Costumi e Travestimenti" },
                    { 13, "Biciclette, monopattini, pattini, palloni, pistole ad acqua, altalene, casette.", "Sport e Giochi all'Aperto" },
                    { 14, "Tastiere elettroniche, chitarre per bambini, xilofoni, batterie, microfoni.", "Musica e Strumenti Giocattolo" },
                    { 15, "Console (Nintendo Switch, PlayStation), videogiochi fisici, accessori gaming, Tamagotchi.", "Videogiochi e Elettronica" },
                    { 16, "Fiabe, libri interattivi, fumetti per ragazzi, dispositivi audio (Fabbrica delle Storie, Tonies).", "Libri, Fumetti e Cantastorie" },
                    { 17, "Zaini con personaggi, astucci, portapranzi, grembiuli.", "Zaini e Articoli Scolastici" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prodotti_CategoriaId",
                table: "Prodotti",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prodotti_Categorie_CategoriaId",
                table: "Prodotti",
                column: "CategoriaId",
                principalTable: "Categorie",
                principalColumn: "CategoriaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prodotti_Categorie_CategoriaId",
                table: "Prodotti");

            migrationBuilder.DropTable(
                name: "Categorie");

            migrationBuilder.DropIndex(
                name: "IX_Prodotti_CategoriaId",
                table: "Prodotti");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Prodotti");
        }
    }
}
