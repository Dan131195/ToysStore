using System.ComponentModel.DataAnnotations;

namespace ToysStore.Models
{
    public class StatoOrdine
    {
        [Key]
        public int StatoOrdineId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        public ICollection<Ordine> Ordini { get; set; } = new List<Ordine>();
    }
}
