using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToysStore.Data;
using ToysStore.DTOs.RecensioneProdotto;
using ToysStore.Models;
using ToysStore.Models.Auth;

namespace ToysStore.Services
{
    public class RecensioneUtenteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RecensioneUtenteService> _logger;

        public RecensioneUtenteService(ApplicationDbContext context, ILogger<RecensioneUtenteService> logger)
        {

            _logger = logger;
        }

        // - GET: Lista recensioni Utente
        public async Task<List<RecensioneUtenteDto>> GetAllRecensioniUtenteAsync(Guid utenteId)
        {
            try
            {
                var recensioni = await _context.RecensioniUtente
                    .Include(r => r.Acquirente)
                    .Where(r => r.VenditoreId == utenteId)
                    .OrderByDescending(r => r.DataRecensione)
                    .Select(r => new RecensioneUtenteDto
                    {
                        RecensioneId = r.RecensioneId,
                        RecensioneTesto = r.RecensioneTesto,
                        Valutazione = r.Valutazione,
                        DataRecensione = r.DataRecensione,
                        Acquirente = r.Acquirente.Nickname,
                    })
                    .ToListAsync();

                _logger.LogInformation($"Recensioni dell'utente {utenteId} caricate con successo.");

                return recensioni;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nel caricamento delle recensioni dell'utente {utenteId}.");
                throw;
            }
        }

        // - POST: Recensione Utente
        public async Task<bool> AddRecensioneUtenteAsync(CreateRecensioneUtenteDto dto, string userId)
        {
            try
            {
                var user = await _context.Utenti
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null) return false;

                var ordine = await _context.Ordini
                    .FirstOrDefaultAsync(o => o.OrdineId == dto.OrdineId);
                if (ordine == null || ordine.UserId != userId) return false;

                // Controlla se l'ordine collegato ha già una recensione
                bool controlloRecensioneOrdine = await _context.RecensioniUtente
                    .AnyAsync(r => r.OrdineId == dto.OrdineId);

                if (controlloRecensioneOrdine) return false;

                var recensione = new RecensioneUtente
                {
                    RecensioneId = Guid.NewGuid(),
                    OrdineId = dto.OrdineId,
                    VenditoreId = dto.VenditoreId,
                    AcquirenteId = user.UtenteId,
                    Valutazione = dto.Valutazione,
                    RecensioneTesto = dto.RecensioneTesto,
                    DataRecensione = DateTime.UtcNow
                };

                _logger.LogInformation("Recensione caricata con successo.");

                _context.RecensioniUtente.Add(recensione);
                await _context.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Errore nella creazione della recensione");
                throw;
            }

        }
    }
}
