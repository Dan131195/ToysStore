using System.ComponentModel.DataAnnotations;

namespace ToysStore.Models
{
    public class Condizione
    {
        [Key]
        public int CondizioneId { get; set; }

        [Required]
        [MaxLength(50)]
        public string NomeCondizione { get; set; }

        [Required]
        [MaxLength(500)]
        public string DescrizioneCondizione { get; set; }

        public ICollection<Prodotto> Prodotti { get; set; } = new List<Prodotto>();
    }
}
