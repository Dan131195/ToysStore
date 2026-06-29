using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToysStore.Data;
using ToysStore.DTOs.Utente;
using ToysStore.DTOs.IndirizzoUtente;
using ToysStore.Models;
using ToysStore.Models.Auth;

namespace ToysStore.Services
{
    public class UtenteService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UtenteService> _logger;
        private readonly IWebHostEnvironment _environment;

        public UtenteService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<UtenteService> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _environment = environment;
        }

        // --- Immagine Profilo

        public async Task<string?> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "utenti");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return Path.Combine("uploads", "utenti", uniqueFileName).Replace("\\", "/");
        }

        // --- GET utente

        public async Task<Utente?> GetUtenteByUserIdAsync(string userId)
        {
            return await _context.Utenti
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        // --- PUT utente
        public async Task<bool> UpdateUtenteAsync(string userId, UtenteUpdateDto dto)
        {
            var utente = await _context.Utenti
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (utente == null) return false;

            utente.User.FirstName = dto.Nome;
            utente.User.LastName = dto.Cognome;

            if (utente.User.Email != dto.Email)
            {
                utente.User.Email = dto.Email;
                utente.User.UserName = dto.Email;
            }

            utente.Nickname = dto.NickName;

            if (dto.ImmagineProfilo != null)
            {
                if (!string.IsNullOrEmpty(utente.ImmagineProfilo))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, utente.ImmagineProfilo);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                utente.ImmagineProfilo = await SaveImageAsync(dto.ImmagineProfilo);
            }

            await _userManager.UpdateAsync(utente.User);
            await _context.SaveChangesAsync();

            return true;
        }

        // --- DELETE utente
        public async Task<IdentityResult?> DeleteUtenteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var utenteProfilo = await _context.Utenti.FirstOrDefaultAsync(u => u.UserId == userId);
            if (utenteProfilo != null && !string.IsNullOrEmpty(utenteProfilo.ImmagineProfilo))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, utenteProfilo.ImmagineProfilo);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            var result = await _userManager.DeleteAsync(user);

            return result;
        }

        // --- GET Indirizzi dell'utente
        public async Task<IEnumerable<IndirizzoUtenteResponseDto>> GetIndirizziByUserIdAsync(string userId)
        {
            return await _context.IndirizziUtenti
                .Where(i => i.UserId == userId)
                .Select(i => new IndirizzoUtenteResponseDto
                {
                    IndirizzoId = i.IndirizzoId,
                    Via = i.Via,
                    Citta = i.Citta,
                    CAP = i.CAP,
                    Provincia = i.Provincia,
                    NomeIndirizzo = i.NomeIndirizzo,
                    IsPredefinito = i.IsPredefinito
                })
                .ToListAsync();
        }

        // --- Disattivazione indirizzo predefinito 
        private async Task DisattivaPredefinitiEsistentiAsync(string userId)
        {
            var predefiniti = await _context.IndirizziUtenti
                .Where(i => i.UserId == userId && i.IsPredefinito)
                .ToListAsync();

            foreach (var p in predefiniti)
            {
                p.IsPredefinito = false;
            }
        }

        // --- POST Indirizzo Utente
        public async Task<IndirizzoUtenteResponseDto?> AddIndirizzoAsync(string userId, IndirizzoUtenteCreateDto dto)
        {
            var userExists = await _userManager.FindByIdAsync(userId);
            if (userExists == null) return null;

            if (dto.IsPredefinito)
            {
                await DisattivaPredefinitiEsistentiAsync(userId);
            }
            else
            {
                var haGiaIndirizzi = await _context.IndirizziUtenti.AnyAsync(i => i.UserId == userId);
                if (!haGiaIndirizzi)
                {
                    dto.IsPredefinito = true;
                }
            }

            var nuovoIndirizzo = new IndirizzoUtente
            {
                IndirizzoId = Guid.NewGuid(),
                UserId = userId,
                Via = dto.Via,
                Citta = dto.Citta,
                CAP = dto.CAP,
                Provincia = dto.Provincia,
                NomeIndirizzo = dto.NomeIndirizzo,
                IsPredefinito = dto.IsPredefinito
            };

            _context.IndirizziUtenti.Add(nuovoIndirizzo);
            await _context.SaveChangesAsync();

            return new IndirizzoUtenteResponseDto
            {
                IndirizzoId = nuovoIndirizzo.IndirizzoId,
                Via = nuovoIndirizzo.Via,
                Citta = nuovoIndirizzo.Citta,
                CAP = nuovoIndirizzo.CAP,
                Provincia = nuovoIndirizzo.Provincia,
                NomeIndirizzo = nuovoIndirizzo.NomeIndirizzo,
                IsPredefinito = nuovoIndirizzo.IsPredefinito
            };
        }

        // --- PUT Indirizzo Utente
        public async Task<bool> UpdateIndirizzoAsync(string userId, Guid indirizzoId, IndirizzoUtenteUpdateDto dto)
        {
            var indirizzo = await _context.IndirizziUtenti
                .FirstOrDefaultAsync(i => i.IndirizzoId == indirizzoId && i.UserId == userId);

            if (indirizzo == null) return false;

            if (dto.IsPredefinito && !indirizzo.IsPredefinito)
            {
                await DisattivaPredefinitiEsistentiAsync(userId);
            }

            indirizzo.Via = dto.Via;
            indirizzo.Citta = dto.Citta;
            indirizzo.CAP = dto.CAP;
            indirizzo.Provincia = dto.Provincia;
            indirizzo.NomeIndirizzo = dto.NomeIndirizzo;
            indirizzo.IsPredefinito = dto.IsPredefinito;

            await _context.SaveChangesAsync();
            return true;
        }

        // --- DELETE Indirizzo Utente
        public async Task<bool> DeleteIndirizzoAsync(string userId, Guid indirizzoId)
        {
            var indirizzo = await _context.IndirizziUtenti
                .FirstOrDefaultAsync(i => i.IndirizzoId == indirizzoId && i.UserId == userId);

            if (indirizzo == null) return false;

            _context.IndirizziUtenti.Remove(indirizzo);
            await _context.SaveChangesAsync();

            if (indirizzo.IsPredefinito)
            {
                var prossimoIndirizzo = await _context.IndirizziUtenti
                    .Where(i => i.UserId == userId)
                    .FirstOrDefaultAsync();

                if (prossimoIndirizzo != null)
                {
                    prossimoIndirizzo.IsPredefinito = true;
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }
    }
}
