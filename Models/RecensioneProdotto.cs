using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToysStore.Models.Auth;

namespace ToysStore.Models
{
    public class RecensioneProdotto
    {
        [Key]
        public Guid RecensioneId { get; set; }

        [MaxLength(1000)]
        public string? RecensioneTesto { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La valutazione deve essere tra 1 e 5")]
        public int Valutazione { get; set; }

        [Required]
        public Guid ProdottoId { get; set; }

        [ForeignKey("ProdottoId")]
        public Prodotto Prodotto { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime DataRecensione { get; set; }
    }
}
