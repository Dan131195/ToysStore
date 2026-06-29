namespace ToysStore.DTOs.Utente
{
    public class UtenteResponseDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string? ImmagineProfiloUrl { get; set; }
    }
}
