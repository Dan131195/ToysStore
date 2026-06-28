using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToysStore.Models.Auth;

namespace ToysStore.Models
{
    public class IndirizzoUtente
    {
        [Key]
        public Guid IndirizzoId { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(200)]
        public string Indirizzo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Citta { get; set; }

        [Required]
        [MaxLength(10)]
        public string CAP { get; set; }

        [MaxLength(50)]
        public string Provincia { get; set; }

        [MaxLength(50)]
        public string? NomeIndirizzo { get; set; } 

        public bool IsPredefinito { get; set; } = false;

    }
}
