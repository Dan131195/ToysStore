using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToysStore.Models
{
    public class ImmagineProdotto
    {
        [Key]
        public Guid ImmagineId { get; set; }

        [Required]
        public string UrlImmagine { get; set; }

        public string? AltText { get; set; }

        public Guid ProdottoId { get; set; }

        [ForeignKey("ProdottoId")]
        public Prodotto Prodotto { get; set; }

    }
}

