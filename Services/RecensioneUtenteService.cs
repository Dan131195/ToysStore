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
        public async Task<RecensioneUtente?> GetAllRecensioniUtente(string userId, Guid utenteId)
        {
            try
            {
                var recensioni = await _context.Utenti
                    .Include(u => u.UtenteId)
                    .ToListAsync();

                _logger.LogInformation($"Recensioni dell'utente {utenteId} caricate con successo da user {userId}");

                return recensioni;
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Errore nel caricamento delle recensioni dell'utente {utenteId} da user {userId}");
                throw;
            }
        }
    }
}
