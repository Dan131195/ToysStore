namespace ToysStore.DTOs.RecensioneProdotto
{
    public class CreateRecensioneUtenteDto
    {
        public Guid OrdineId { get; set; }
        public Guid VenditoreId { get; set; }
        public Guid AcquirenteId { get; set; }
        public string? RecensioneTesto { get; set; }
        public int Valutazione { get; set; }

    }
}
