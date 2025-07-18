using System.ComponentModel.DataAnnotations;
using ToysStore.Models.Auth;

namespace ToysStore.Models
{
    public class Prodotto
    {
        [Key]
        public Guid GiocattoloId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? NomeGiocattolo { get;set; }

        [Required]
        public string? ImmagineGiocattolo { get; set; }

        [Required]
        public string? ImmagineGiocattoloDue { get; set; }
        
        public string? ImmagineGiocattoloTre { get; set; }
        
        public string? ImmagineGiocattoloQuattro { get; set; }

        [Required]
        [MaxLength(2000)] 
        public string? DescrizioneGiocattolo { get; set; }
        [Required]
        public string? Condizioni { get; set; }

        [Required]
        public decimal PrezzoGiocattolo { get; set; }

        public Guid UtenteId { get; set; }
        public ApplicationUser? Utente { get; set; }

    }
}
