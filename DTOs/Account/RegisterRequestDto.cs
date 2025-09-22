using System.ComponentModel.DataAnnotations;

namespace ToysStore.DTOs.Account
{
    public class RegisterRequestDto
    {
        
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Ruolo { get; set; }
    }
}