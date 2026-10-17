using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToysStore.Models.Auth;

namespace ToysStore.Models
{
    public class Ordine
    {
        [Key]
        public Guid OrdineId { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il totale deve essere maggiore di 0")]
        public decimal Totale { get; set; }

        [Required]
        [MaxLength(200)]
        public string IndirizzoSnapshot { get; set; }

        [MaxLength(100)]
        public string? CittaSnapshot { get; set; }

        [MaxLength(10)]
        public string? CAPSnapshot { get; set; }
        public DateTime DataOrdine { get; set; } = DateTime.UtcNow;
        public ICollection<ProdottoOrdine> ProdottiOrdine { get; set; } = new List<ProdottoOrdine>();

        [Required]
        public int StatoOrdineId { get; set; }
        [ForeignKey("StatoOrdineId")]
        public StatoOrdine StatoOrdine { get; set; }

        public RecensioneUtente? Recensione { get; set; }
    }
}
