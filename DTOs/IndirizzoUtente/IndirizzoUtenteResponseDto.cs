namespace ToysStore.DTOs.IndirizzoUtente
{
    public class IndirizzoUtenteResponseDto
    {
        public Guid IndirizzoId { get; set; }
        public string Via { get; set; } = string.Empty;
        public string Citta { get; set; } = string.Empty;
        public string CAP { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string? NomeIndirizzo { get; set; }
        public bool IsPredefinito { get; set; }
    }
}
