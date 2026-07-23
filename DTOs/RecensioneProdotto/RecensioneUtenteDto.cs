namespace ToysStore.DTOs.RecensioneProdotto
{
    public class RecensioneUtenteDto
    {
        public Guid RecensioneId { get; set; }
        public string? RecensioneTesto { get; set; }
        public int Valutazione { get; set; }
        public DateTime DataRecensione { get; set; }
        public string Acquirente { get; set; }

    }
}
