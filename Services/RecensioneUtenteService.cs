using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<List<RecensioneUtenteDto>> GetAllRecensioniUtente(Guid utenteId)
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
        public async Task<bool> AddRecensioneUtente(CreateRecensioneUtenteDto dto)
        {
            try
            {
                // Controlla se l'ordine collegato ha già una recensione
                bool controlloRecensioneOrdine = await _context.RecensioniUtente
                    .AnyAsync(r => r.OrdineId == dto.OrdineId);

                if (controlloRecensioneOrdine) return false;

                var recensione = new RecensioneUtente
                {
                    RecensioneId = Guid.NewGuid(),
                    OrdineId = dto.OrdineId,
                    VenditoreId = dto.VenditoreId,
                    AcquirenteId = dto.AcquirenteId,
                    Valutazione = dto.Valutazione,
                    RecensioneTesto = dto.RecensioneTesto,
                    DataRecensione = DateTime.UtcNow
                };

                _logger.LogInformation("Recensione caricata con successo.");

                _context.RecensioniUtente.Add(recensione);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Errore nella creazione della recensione");
                throw;
            }

        }
    }
}
