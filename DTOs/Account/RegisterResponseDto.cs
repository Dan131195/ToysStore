namespace ToysStore.DTOs.Account
{
    public class RegisterResponseDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Ruolo { get; set; }
        public Guid? UtenteId { get; set; }
    }
}
