using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [MaxLength(2000)] 
        public string? DescrizioneGiocattolo { get; set; }

        [Required]
        public string? Condizioni { get; set; }

        [Required]
        public decimal PrezzoGiocattolo { get; set; }   
        
        public Guid UtenteId { get; set; }
        [ForeignKey("UtenteId")]
        public ApplicationUser? Utente { get; set; }
        public ICollection<ImmagineProdotto> ImmaginiProdotto { get; set; } = new List<ImmagineProdotto>();
        public ICollection<RecensioneProdotto> RecensioniProdotto { get; set; } = new List<RecensioneProdotto>();
    }
}
