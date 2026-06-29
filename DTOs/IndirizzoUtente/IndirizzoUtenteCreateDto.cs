using System.ComponentModel.DataAnnotations;

namespace ToysStore.DTOs.IndirizzoUtente
{
    public class IndirizzoUtenteCreateDto
    {
        [Required]
        [MaxLength(200, ErrorMessage = "L'indirizzo non può superare i 200 caratteri.")]
        public string Via { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "La città non può superare i 100 caratteri.")]
        public string Citta { get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage = "Il CAP non può superare i 10 caratteri.")]
        public string CAP { get; set; } = string.Empty;

        [Required]
        [MaxLength(50, ErrorMessage = "La provincia non può superare i 50 caratteri.")]
        public string Provincia { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Il nome dell'indirizzo non può superare i 50 caratteri.")]
        public string? NomeIndirizzo { get; set; }

        public bool IsPredefinito { get; set; } = false;
    }
}
