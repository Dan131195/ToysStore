using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToysStore.Migrations
{
    /// <inheritdoc />
    public partial class Indirizzo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prodotti_Condizione_CondizioneId",
                table: "Prodotti");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Condizione",
                table: "Condizione");

            migrationBuilder.RenameTable(
                name: "Condizione",
                newName: "Condizioni");

            migrationBuilder.RenameColumn(
                name: "Indirizzo",
                table: "IndirizziUtenti",
                newName: "Via");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Condizioni",
                table: "Condizioni",
                column: "CondizioneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prodotti_Condizioni_CondizioneId",
                table: "Prodotti",
                column: "CondizioneId",
                principalTable: "Condizioni",
                principalColumn: "CondizioneId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prodotti_Condizioni_CondizioneId",
                table: "Prodotti");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Condizioni",
                table: "Condizioni");

            migrationBuilder.RenameTable(
                name: "Condizioni",
                newName: "Condizione");

            migrationBuilder.RenameColumn(
                name: "Via",
                table: "IndirizziUtenti",
                newName: "Indirizzo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Condizione",
                table: "Condizione",
                column: "CondizioneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prodotti_Condizione_CondizioneId",
                table: "Prodotti",
                column: "CondizioneId",
                principalTable: "Condizione",
                principalColumn: "CondizioneId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
