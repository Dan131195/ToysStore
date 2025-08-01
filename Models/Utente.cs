using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToysStore.Models.Auth;

namespace ToysStore.Models
{
    public class Utente
    {
        [Key]
        public Guid UtenteId { get; set; }

        [Required]
        public string? Nickname { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
    }
}
