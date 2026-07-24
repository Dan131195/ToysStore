namespace ToysStore.DTOs
{
    public class CreaOrdineDto
    {
        public decimal Totale { get; set; }
        public string IndirizzoSnapshot { get; set; } = string.Empty;
        public string? CittaSnapshot { get; set; }
        public string? CAPSnapshot { get; set; }
    }
}