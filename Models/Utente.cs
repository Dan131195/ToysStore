using System.ComponentModel.DataAnnotations;

namespace ToysStore.Models
{
    public class Utente
    {
        [Key]
        public int UtenteId { get; set; }

        [Required]
        public string? UtenteNome { get; set; }

        [Required]
        public string? UtenteCognome { get; set; }

        [Required]
        public string? Indirizzo { get; set; }
    }
}
