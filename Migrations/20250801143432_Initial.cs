using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatiOrdine",
                columns: table => new
                {
                    StatoOrdineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatiOrdine", x => x.StatoOrdineId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndirizziUtenti",
                columns: table => new
                {
                    IndirizzoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Indirizzo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Citta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CAP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NomeIndirizzo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPredefinito = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndirizziUtenti", x => x.IndirizzoId);
                    table.ForeignKey(
                        name: "FK_IndirizziUtenti_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prodotti",
                columns: table => new
                {
                    GiocattoloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeGiocattolo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescrizioneGiocattolo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Condizioni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrezzoGiocattolo = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodotti", x => x.GiocattoloId);
                    table.ForeignKey(
                        name: "FK_Prodotti_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.UtenteId);
                    table.ForeignKey(
                        name: "FK_Utenti_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImmaginiProdotto",
                columns: table => new
                {
                    ImmagineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UrlImmagine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImmaginiProdotto", x => x.ImmagineId);
                    table.ForeignKey(
                        name: "FK_ImmaginiProdotto_Prodotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "Prodotti",
                        principalColumn: "GiocattoloId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ordini",
                columns: table => new
                {
                    OrdineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Totale = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IndirizzoSnapshot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CittaSnapshot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CAPSnapshot = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IndirizzoUtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatoOrdineId = table.Column<int>(type: "int", nullable: false),
                    DataOrdine = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProdottoGiocattoloId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordini", x => x.OrdineId);
                    table.ForeignKey(
                        name: "FK_Ordini_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ordini_IndirizziUtenti_IndirizzoUtenteId",
                        column: x => x.IndirizzoUtenteId,
                        principalTable: "IndirizziUtenti",
                        principalColumn: "IndirizzoId");
                    table.ForeignKey(
                        name: "FK_Ordini_Prodotti_ProdottoGiocattoloId",
                        column: x => x.ProdottoGiocattoloId,
                        principalTable: "Prodotti",
                        principalColumn: "GiocattoloId");
                    table.ForeignKey(
                        name: "FK_Ordini_StatiOrdine_StatoOrdineId",
                        column: x => x.StatoOrdineId,
                        principalTable: "StatiOrdine",
                        principalColumn: "StatoOrdineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdottiCarrello",
                columns: table => new
                {
                    ProdottoCarrelloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantita = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdottiCarrello", x => x.ProdottoCarrelloId);
                    table.ForeignKey(
                        name: "FK_ProdottiCarrello_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdottiCarrello_Prodotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "Prodotti",
                        principalColumn: "GiocattoloId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecensioniProdotto",
                columns: table => new
                {
                    RecensioneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecensioneTesto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Valutazione = table.Column<int>(type: "int", nullable: false),
                    ProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataRecensione = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ProdottiOrdine",
                columns: table => new
                {
                    ProdottoOrdineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantita = table.Column<int>(type: "int", nullable: false),
                    PrezzoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdottiOrdine", x => x.ProdottoOrdineId);
                    table.ForeignKey(
                        name: "FK_ProdottiOrdine_Ordini_OrdineId",
                        column: x => x.OrdineId,
                        principalTable: "Ordini",
                        principalColumn: "OrdineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdottiOrdine_Prodotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "Prodotti",
                        principalColumn: "GiocattoloId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "StatiOrdine",
                columns: new[] { "StatoOrdineId", "Nome" },
                values: new object[,]
                {
                    { 1, "In Attesa" },
                    { 2, "Confermato" },
                    { 3, "In Preparazione" },
                    { 4, "Spedito" },
                    { 5, "Ricevuto" },
                    { 6, "Annullato" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ImmaginiProdotto_ProdottoId",
                table: "ImmaginiProdotto",
                column: "ProdottoId");

            migrationBuilder.CreateIndex(
                name: "IX_IndirizziUtenti_UserId",
                table: "IndirizziUtenti",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_IndirizzoUtenteId",
                table: "Ordini",
                column: "IndirizzoUtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_ProdottoGiocattoloId",
                table: "Ordini",
                column: "ProdottoGiocattoloId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_StatoOrdineId",
                table: "Ordini",
                column: "StatoOrdineId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_UserId",
                table: "Ordini",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Prodotti_UserId",
                table: "Prodotti",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdottiCarrello_ProdottoId",
                table: "ProdottiCarrello",
                column: "ProdottoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdottiCarrello_UserId",
                table: "ProdottiCarrello",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdottiOrdine_OrdineId",
                table: "ProdottiOrdine",
                column: "OrdineId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdottiOrdine_ProdottoId",
                table: "ProdottiOrdine",
                column: "ProdottoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecensioniProdotto_ProdottoId",
                table: "RecensioniProdotto",
                column: "ProdottoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecensioniProdotto_UserId",
                table: "RecensioniProdotto",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Utenti_UserId",
                table: "Utenti",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ImmaginiProdotto");

            migrationBuilder.DropTable(
                name: "ProdottiCarrello");

            migrationBuilder.DropTable(
                name: "ProdottiOrdine");

            migrationBuilder.DropTable(
                name: "RecensioniProdotto");

            migrationBuilder.DropTable(
                name: "Utenti");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Ordini");

            migrationBuilder.DropTable(
                name: "IndirizziUtenti");

            migrationBuilder.DropTable(
                name: "Prodotti");

            migrationBuilder.DropTable(
                name: "StatiOrdine");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
