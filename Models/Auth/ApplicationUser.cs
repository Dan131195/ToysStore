using System.ComponentModel.DataAnnotations;
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

        public ICollection<Prodotto>? ListaGiocattoli {  get; set; }
        public ICollection<Carrello>? Carrelli { get; set; } 
    }
}
