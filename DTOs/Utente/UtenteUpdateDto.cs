namespace ToysStore.DTOs.Utente
{
    public class UtenteUpdateDto
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public IFormFile? ImmagineProfilo { get; set; }
    }
}
