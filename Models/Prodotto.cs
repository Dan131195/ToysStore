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
        
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public ICollection<ImmagineProdotto> ImmaginiProdotto { get; set; } = new List<ImmagineProdotto>();
        public ICollection<RecensioneProdotto> RecensioniProdotto { get; set; } = new List<RecensioneProdotto>();
        public ICollection<ProdottoCarrello>? ProdottiCarrello { get; set; }
        public ICollection<ProdottoOrdine>? ProdottiOrdine { get; set; } 
        public ICollection<Ordine> Ordini {  get; set; }
    }
}
