using System.ComponentModel.DataAnnotations;

namespace ToysStore.DTOs.IndirizzoUtente
{
    public class IndirizzoUtenteUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Via { get; set; }

        [Required]
        [MaxLength(100)]
        public string Citta { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string CAP { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Provincia { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? NomeIndirizzo { get; set; }

        public bool IsPredefinito { get; set; } = false;
    }
}
