namespace ToysStore.DTOs.Utente
{
    public class UtenteCreateResponseDto
    {
        public Guid UtenteId { get; set; }
        public string Nickname { get; set; }
        public string UserId { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
    }
}
