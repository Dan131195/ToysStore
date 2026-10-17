using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToysStore.Models
{
    public class ProdottoOrdine
    {
        [Key] 
        public int ProdottoOrdineId { get; set; }

        [Required]
        public Guid OrdineId { get; set; }
        [ForeignKey("OrdineId")]
        public Ordine Ordine { get; set; }

        [Required]
        public Guid ProdottoId { get; set; }
        [ForeignKey("ProdottoId")]
        public Prodotto Prodotto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La quantità deve essere almeno 1")]
        public int Quantita { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo unitario deve essere maggiore di 0")]
        public decimal PrezzoUnitario { get; set; }

        [NotMapped]
        public decimal Subtotale => Quantita * PrezzoUnitario;
    }
}
