using ToysStore.Models.Auth;

namespace ToysStore.Models
{
    public class Carrello
    {
        public Guid CArrelloId { get; set; }

        public DateTime CreazioneCarrello { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<ProdottoCarrello> Items { get; set; } = new List<ProdottoCarrello>();
    }
}
