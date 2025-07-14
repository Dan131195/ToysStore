using System.ComponentModel.DataAnnotations;

namespace ToysStore.Models
{
    public class Giocattolo
    {
        [Key]
        public Guid GiocattoloId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NomeGiocattolo { get;set; }

        [Required]
        public string ImmagineGiocattolo { get; set; }

        [Required]
        public string ImmagineGiocattoloDue { get; set; }
        
        public string ImmagineGiocattoloTre { get; set; }
        
        public string ImmagineGiocattoloQuattro { get; set; }

        [Required]
        [MaxLength(2000)]
        public string DescrizioneGiocattolo { get; set; }

        [Required]
        public decimal PrezzoGiocattolo { get; set; }



    }
}
