using Microsoft.EntityFrameworkCore;
using ToysStore.Data;
using ToysStore.DTOs.RecensioneProdotto;
using ToysStore.Models;

namespace ToysStore.Services
{
    public class RecensioneUtenteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RecensioneUtenteService> _logger;

        public RecensioneUtenteService(ApplicationDbContext context, ILogger<RecensioneUtenteService> logger)
        {

            _context = context;
            _logger = logger;
        }


        // - GET: Lista recensioni Utente
        public async Task<List<RecensioneUtenteResponseDto>> GetAllRecensioniUtenteAsync(Guid utenteId)
        {
            try
            {
                var recensioni = await _context.RecensioniUtente
                    .Include(r => r.Acquirente)
                    .Where(r => r.VenditoreId == utenteId)
                    .OrderByDescending(r => r.DataRecensione)
                    .Select(r => new RecensioneUtenteResponseDto
                    {
                        RecensioneId = r.RecensioneId,
                        RecensioneTesto = r.RecensioneTesto,
                        Valutazione = r.Valutazione,
                        DataRecensione = r.DataRecensione,
                        Acquirente = r.Acquirente.Nickname,
                        Venditore = r.Venditore.Nickname,
                        NomiProdotti = r.Ordine.ProdottiOrdine
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
        public async Task<RecensioneUtenteResponseDto> AddRecensioneUtenteAsync(RecensioneUtenteCreateDto dto, string userId)
        {
            try
            {
                var user = await _context.Utenti
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null) return null;

                var ordine = await _context.Ordini
                    .FirstOrDefaultAsync(o => o.OrdineId == dto.OrdineId);
                if (ordine == null || ordine.UserId != userId) return null;

                var venditore = await _context.Utenti
                    .FirstOrDefaultAsync(u => u.UtenteId == dto.VenditoreId);
                if (venditore == null) return null;

                // Controlla se l'ordine collegato ha già una recensione
                bool controlloRecensioneOrdine = await _context.RecensioniUtente
                    .AnyAsync(r => r.OrdineId == dto.OrdineId);

                if (controlloRecensioneOrdine) return null;

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

                _context.RecensioniUtente.Add(recensione);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Recensione caricata con successo.");

                var response = new RecensioneUtenteResponseDto
                {
                    RecensioneId = recensione.RecensioneId,
                    RecensioneTesto = recensione.RecensioneTesto,
                    Valutazione = recensione.Valutazione,
                    DataRecensione = recensione.DataRecensione,
                    Acquirente = user.Nickname,
                    Venditore = venditore.Nickname,
                    NomiProdotti = ordine.ProdottiOrdine
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nella creazione della recensione");
                throw;
            }
        }

        // - PUT: Recensione Utente
        public async Task<RecensioneUtenteResponseDto?> UpdateRecensioneUtenteAsync(RecensioneUtenteUpdateDto dto, Guid userId, Guid RecensioneId)
        {
            try
            {
                var recensione = await _context.RecensioniUtente
                    .Include(r => r.Acquirente)
                    .Include(r => r.Venditore)
                    .Include(r => r.Ordine)
                    .FirstOrDefaultAsync(r => r.RecensioneId == RecensioneId && r.AcquirenteId == userId);

                if (recensione == null) return null;

                recensione.RecensioneTesto = dto.RecensioneTesto;
                recensione.Valutazione = dto.Valutazione;
                recensione.DataRecensione = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var response = new RecensioneUtenteResponseDto
                {
                    RecensioneId = recensione.RecensioneId,
                    RecensioneTesto = recensione.RecensioneTesto,
                    Valutazione = recensione.Valutazione,
                    DataRecensione = recensione.DataRecensione,
                    Acquirente = recensione.Acquirente.Nickname,
                    Venditore = recensione.Venditore.Nickname,
                    NomiProdotti = recensione.Ordine.ProdottiOrdine
                };

                _logger.LogInformation("Recensione caricata con successo.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante la modifica della recensione {RecensioneId}");
                throw;
            }
        }

        // - DELETE: Recensione utente
        public async Task<bool> DeleteRecensioneUtente(Guid userId, Guid recensioneId)
        {
            try
            {
                var recensione = await _context.RecensioniUtente
                    .FirstOrDefaultAsync(r => r.RecensioneId == recensioneId && r.AcquirenteId == userId);
                if (recensione == null) return false;

                _context.RecensioniUtente.Remove(recensione);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Recensione {recensioneId} utente {userId} cancellata con successo.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nella cencellazione della recensione {recensioneId}");
                throw;
            }
        }
    }
}
