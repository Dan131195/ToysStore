using Microsoft.AspNetCore.Identity;

namespace ToysStore.Models.Auth
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole>? UserRoles { get; set; }
    }
}
