using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ToysStore.Models.Auth;

namespace ToysStore.Models
{
    public class Utente
    {
        [Key]
        public Guid UtenteId { get; set; }

        [Required]
        public string? Nickname { get; set; }

        public string? ImmagineProfilo { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<RecensioneUtente> RecensioniRicevute { get; set; } = new List<RecensioneUtente>();
        public ICollection<RecensioneUtente> RecensioniScritte { get; set; } = new List<RecensioneUtente>();
        
    }
}
