using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToysStore.Models.Auth;

namespace ToysStore.Models
{
    public class RecensioneUtente
    {
        [Key]
        public Guid RecensioneId { get; set; }

        [MaxLength(1000)]
        public string? RecensioneTesto { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La valutazione deve essere tra 1 e 5")]
        public int Valutazione { get; set; }

        [Required]
        public DateTime DataRecensione { get; set; }

        [Required]
        public Guid AcquirenteId { get; set; }
        [ForeignKey("AcquirenteId")]
        public Utente Acquirente { get; set; }

        [Required]
        public Guid VenditoreId { get; set; }
        [ForeignKey("VenditoreId")]
        public Utente Venditore { get; set; }

        [Required]
        public Guid OrdineId { get; set; }
        [ForeignKey("OrdineId")]
        public Ordine Ordine { get; set; }

    }
}
