using System.ComponentModel.DataAnnotations;

namespace ToysStore.Models
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NomeCategoria { get; set; }

        [MaxLength(500)]
        public string? DescrizioneCategoria { get; set; }

        public ICollection<Prodotto> Prodotti { get; set; } = new List<Prodotto>();
    }
}