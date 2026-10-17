using ToysStore.Data;
using ToysStore.DTOs;
using ToysStore.Models;

namespace ToysStore.Services
{
    public class OrdineService
    {
        private readonly ApplicationDbContext _context;

        public OrdineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreaOrdineTestAsync(CreaOrdineDto dto, string userId)
        {
            var ordine = new Ordine
            {
                OrdineId = Guid.NewGuid(),
                UserId = userId, // Questo è l'ApplicationUserId (stringa) preso dal token
                Totale = dto.Totale,
                IndirizzoSnapshot = dto.IndirizzoSnapshot,
                CittaSnapshot = dto.CittaSnapshot,
                CAPSnapshot = dto.CAPSnapshot,
                DataOrdine = DateTime.UtcNow,

                // Forziamo lo stato a 5 ("Ricevuto") così puoi testare subito la recensione!
                StatoOrdineId = 5
            };

            _context.Ordini.Add(ordine);
            await _context.SaveChangesAsync();

            return ordine.OrdineId;
        }
    }
}