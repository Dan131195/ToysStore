using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ToysStore.Models.Auth
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        public ICollection<ApplicationUserRole>? UserRoles { get; set; }
        public ICollection<Prodotto>? ListaProdotti {  get; set; }
        public ICollection<ProdottoCarrello>? ProdottiCarrello { get; set; }
        public ICollection<Ordine>? Ordini { get; set; }
        public ICollection<IndirizzoUtente> Indirizzi { get; set; }
        public Utente Utente { get; set; }
    }
}
