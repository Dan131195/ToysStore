using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ToysStore.Models.Auth
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public ApplicationUser? ApplicationUser { get; set; }
        public ApplicationRole? ApplicationRole { get; set; }
    }
}
