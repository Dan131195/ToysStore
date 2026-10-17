using ToysStore.Models;

namespace ToysStore.DTOs.RecensioneProdotto
{
    public class RecensioneUtenteCreateDto
    {
        public Guid OrdineId { get; set; }
        public Guid VenditoreId { get; set; }
        public string? RecensioneTesto { get; set; }
        public int Valutazione { get; set; }
        public string Acquirente { get; set; }
        public string Venditore { get; set; }
        public ICollection<ProdottoOrdine> NomiProdotti { get; set; }

    }
}
