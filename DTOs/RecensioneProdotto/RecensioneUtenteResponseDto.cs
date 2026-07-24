using ToysStore.Models;

namespace ToysStore.DTOs.RecensioneProdotto
{
    public class RecensioneUtenteResponseDto
    {
        public Guid RecensioneId { get; set; }
        public string? RecensioneTesto { get; set; }
        public int Valutazione { get; set; }
        public DateTime DataRecensione { get; set; }
        public string Acquirente { get; set; }
        public string Venditore { get; set; }
        public ICollection<ProdottoOrdine> NomiProdotti { get; set; }
    }
}
