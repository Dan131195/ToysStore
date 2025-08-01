using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToysStore.Models
{
    public class Utente
    {
        [Key]
        public Guid UtenteId { get; set; }

        [Required]
        public string? Nickname { get; set; }

        [Required]
        public string? Indirizzo { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

    }
}
