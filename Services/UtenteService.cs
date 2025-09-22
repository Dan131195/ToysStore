using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToysStore.Data;
using ToysStore.DTOs.Utente;
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

        // Immagine Profilo

        public async Task<string?> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) 
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "updloads", "utenti");
            if(!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return Path.Combine("uploads", "utenti", uniqueFileName).Replace("\\", "/");
        }

        // CREATE Utente

        public async Task<UtenteCreateResponseDto?> CreateAsync(UtenteCreateDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.UserId);
            if (user == null) return null;

            var clienteEsistente = await _context.Utenti.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
            if (clienteEsistente == null) return null;

            var utente = new Utente
            {
                UtenteId = Guid.NewGuid(),
                Nickname = dto.Nickname,
                UserId = dto.UserId
            };

            _context.Utenti.Add(utente);
            await _context.SaveChangesAsync();

            return new UtenteCreateResponseDto
            {
                UtenteId = utente.UtenteId,
                Nickname = utente.Nickname,
                UserId = utente.UserId,
                Email = user.Email,
                Nome = user.FirstName,
                Cognome = user.LastName

            };
        }

        // UPDATE Utente

        public async Task<bool> UpdateUtenteAsync(Guid id, UtenteUpdateDto dto)
        {
            var utente = await _context.Utenti.Include(u =>  u.UserId).FirstOrDefaultAsync(u => u.UtenteId == id);
            if (utente == null) return false;

            utente.User.FirstName = dto.Nome;
            utente.User.LastName = dto.Cognome;
            utente.User.Email = dto.Email;
            utente.Nickname = dto.NickName;
            if (dto.ImmagineProfilo != null)
            {
                var imagePath = await SaveImageAsync(dto.ImmagineProfilo);
                utente.ImmagineProfilo = imagePath;
            }
            await _userManager.UpdateAsync(utente.User);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
