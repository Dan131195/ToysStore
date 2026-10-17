using Microsoft.EntityFrameworkCore;
using ToysStore.Data;
using ToysStore.DTOs.Prodotto;
using ToysStore.Models;

namespace ToysStore.Services
{
    public class ProdottoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProdottoService> _logger;
        private readonly IWebHostEnvironment _environment;

        public ProdottoService(ApplicationDbContext context, ILogger<ProdottoService> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        private ProdottoResponseDto MapToResponseDto(Prodotto p)
        {
            return new ProdottoResponseDto
            {
                GiocattoloId = p.GiocattoloId,
                NomeGiocattolo = p.NomeGiocattolo,
                DescrizioneGiocattolo = p.DescrizioneGiocattolo,
                PrezzoGiocattolo = p.PrezzoGiocattolo,
                CategoriaNome = p.Categoria?.NomeCategoria ?? "Sconosciuta",
                CondizioneNome = p.Condizione?.NomeCondizione ?? "Sconosciuta",
                VenditoreId = p.UserId,
                UrlsImmagini = p.ImmaginiProdotto?.Select(img => img.UrlImmagine).ToList() ?? new List<string>()
            };
        }


        // - GET: Lista tutti prodotti
        public async Task<IEnumerable<ProdottoResponseDto>> GetAllProdottiAsync()
        {
            try
            {
                var prodotti = await _context.Prodotti
                .Include(p => p.Categoria)
                .Include(p => p.Condizione)
                .Include(p => p.ImmaginiProdotto)
                .ToListAsync();

                _logger.LogInformation($"Prodotti caricati con successo");

                return prodotti.Select(MapToResponseDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero prodotti");
                throw;
            }

        }

        // - GET: Prodotti di un singolo utente 
        public async Task<IEnumerable<ProdottoResponseDto>> GetProdottiByUserIdAsync(string userId)
        {
            try
            {
                var prodotti = await _context.Prodotti
                        .Where(p => p.UserId == userId)
                        .Include(p => p.Categoria)
                        .Include(p => p.Condizione)
                        .Include(p => p.ImmaginiProdotto)
                        .ToListAsync();

                _logger.LogInformation($"Prodotti del venditore {userId} caricati con successo");

                return prodotti.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nel recupero prodotti dell'utente {userId}");
                throw;
            }
        }

        // - GET: Singolo prodotto tramite ID 
        public async Task<ProdottoResponseDto?> GetProdottoByIdAsync(Guid id)
        {
            try
            {
                var prodotto = await _context.Prodotti
                                .Include(p => p.Categoria)
                                .Include(p => p.Condizione)
                                .Include(p => p.ImmaginiProdotto)
                                .FirstOrDefaultAsync(p => p.GiocattoloId == id);

                if (prodotto == null) return null;

                _logger.LogInformation($"Prodotto {id} caricato con successo");

                return MapToResponseDto(prodotto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nel recupero prodotto con Id : {id}");
                throw;
            }
        }

        // - POST: Crea un nuovo prodotto 
        public async Task<ProdottoResponseDto> CreateProdottoAsync(ProdottoCreateDto dto, string userId)
        {
            try
            {
                var newProdotto = new Prodotto
                {
                    GiocattoloId = Guid.NewGuid(),
                    NomeGiocattolo = dto.NomeGiocattolo,
                    DescrizioneGiocattolo = dto.DescrizioneGiocattolo,
                    PrezzoGiocattolo = dto.PrezzoGiocattolo,
                    CategoriaId = dto.CategoriaId,
                    CondizioneId = dto.CondizioneId,
                    UserId = userId,
                    ImmaginiProdotto = new List<ImmagineProdotto>()
                };

                if (dto.Immagini != null && dto.Immagini.Any())
                {
                    // Trova la cartella: wwwroot/uploads/prodotti
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "prodotti");

                    // Se la cartella non esiste, la crea in automatico
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var file in dto.Immagini)
                    {
                        if (file.Length > 0)
                        {
                            Guid idImg = Guid.NewGuid();

                            string fileName = idImg.ToString() + " _ " + newProdotto.NomeGiocattolo;

                            string path = Path.Combine(uploadsFolder, fileName);

                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            newProdotto.ImmaginiProdotto.Add(new ImmagineProdotto
                            {
                                ImmagineId = idImg,
                                UrlImmagine = $"/uploads/prodotti/{fileName}",
                                AltText = $"Immagine di {dto.NomeGiocattolo}"
                            });
                        }
                    }
                }

                _context.Prodotti.Add(newProdotto);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Prodotto {newProdotto.NomeGiocattolo} creato con successo con Id {newProdotto.GiocattoloId} da utente {newProdotto.UserId}");

                return await GetProdottoByIdAsync(newProdotto.GiocattoloId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nella creazione del prodotto");
                throw;
            }

        }

        // - PUT: Modifica un prodotto esistente 
        public async Task<ProdottoResponseDto?> UpdateProdottoAsync(Guid id, ProdottoUpdateDto dto, string userId)
        {
            try
            {
                var prodotto = await _context.Prodotti
                        .Include(p => p.Categoria)
                        .Include(p => p.Condizione)
                        .Include(p => p.ImmaginiProdotto)
                        .FirstOrDefaultAsync(p => p.GiocattoloId == id);

                if (prodotto == null) return null;

                if (prodotto.UserId != userId) throw new UnauthorizedAccessException("Non puoi modificare questo annuncio.");

                prodotto.NomeGiocattolo = dto.NomeGiocattolo;
                prodotto.DescrizioneGiocattolo = dto.DescrizioneGiocattolo;
                prodotto.PrezzoGiocattolo = dto.PrezzoGiocattolo;
                prodotto.CategoriaId = dto.CategoriaId;
                prodotto.CondizioneId = dto.CondizioneId;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Prodotto con id {id} modificato con successo da utente {userId}");

                return await GetProdottoByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante la modifica del prodotto con Id {id} dell'utente {userId}");
                throw;
            }

        }

        // - DELETE: Elimina un prodotto 
        public async Task<bool> DeleteProdottoAsync(Guid id, string userId)
        {
            try
            {
                var prodotto = await _context.Prodotti
                    .Include(p => p.ImmaginiProdotto)
                    .FirstOrDefaultAsync(p => p.GiocattoloId == id);

                if (prodotto == null) return false;
                if (prodotto.UserId != userId) throw new UnauthorizedAccessException("Non puoi modificare questo annuncio");

                if (prodotto.ImmaginiProdotto != null && prodotto.ImmaginiProdotto.Any())
                {
                    foreach (var immagine in prodotto.ImmaginiProdotto)
                    {
                        if (!string.IsNullOrEmpty(immagine.UrlImmagine))
                        {
                            string relativePath = immagine.UrlImmagine.TrimStart('/');
                            relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());

                            string path = Path.Combine(_environment.WebRootPath, relativePath);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                                _logger.LogInformation($"File immagine eliminato {path}");
                            }

                        }
                    }
                }

                _context.Prodotti.Remove(prodotto);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Prodotto con id {id} cancellato con successo da utente {userId}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erore nell'eliminazione del prodotto {id} dell'utente {userId}");
                throw;
            }
        }

        // ------------ IMMAGINI PRODOTTI ------------

        // - POST : Immagine prodotto

        public async Task<ProdottoResponseDto?> AddImmagineProdottoAsync(Guid prodottoId, IEnumerable<IFormFile> img, string userId)
        {
            try
            {
                var prodotto = await _context.Prodotti
                    .Include(p => p.ImmaginiProdotto)
                    .FirstOrDefaultAsync(p => p.GiocattoloId == prodottoId);

                if (prodotto == null) return null;
                if (prodotto.UserId != userId) throw new UnauthorizedAccessException("Non puoi modificare queso annuncio");

                if (img != null && img.Any())
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "prodotti");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    foreach (var file in img)
                    {
                        if (file.Length > 0)
                        {
                            Guid idImg = Guid.NewGuid();

                            string fileName = idImg.ToString() + " _ " + prodotto.GiocattoloId;
                            string path = Path.Combine(uploadsFolder, fileName);

                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            prodotto.ImmaginiProdotto.Add(new ImmagineProdotto
                            {
                                ImmagineId = idImg,
                                UrlImmagine = $"uploads/prodotti/{fileName}",
                                AltText = $"Immagine prodotto {prodotto.NomeGiocattolo}"
                            });
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Immagine aggiunta con successo per il prodotto {prodottoId} utente {userId}.");
                    }
                }
                return MapToResponseDto(prodotto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nell'aggiunta dell'immagine del prodtto {prodottoId} utente {userId}");
                throw;
            }
        }

        // - DELETE :Immagine Prodotto

        public async Task<ProdottoResponseDto?> DeleteImmagineProdotto(Guid prodottoId, Guid imgId, string userId)
        {
            try
            {
                var prodotto = await _context.Prodotti
                    .Include(p => p.ImmaginiProdotto)
                    .FirstOrDefaultAsync(p => p.GiocattoloId == prodottoId);

                if (prodotto == null) return null;
                if (prodotto.UserId != userId) throw new UnauthorizedAccessException("Non puoi modificare questo annuncio");

                var img = prodotto.ImmaginiProdotto.FirstOrDefault(i => i.ImmagineId == imgId);

                if (img == null) return null;
                if (img != null)
                {
                    if (!string.IsNullOrEmpty(img.UrlImmagine))
                    {
                        string relativePath = img.UrlImmagine.TrimStart('/');
                        relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());
                        string path = Path.Combine(_environment.WebRootPath, relativePath);

                        if (System.IO.File.Exists(relativePath))
                        {
                            System.IO.File.Delete(relativePath);
                        }
                    }
                    prodotto.ImmaginiProdotto.Remove(img);
                    _logger.LogInformation($"Immagine {imgId} del prodotto {prodottoId} utente {userId} cancellata con successo.");
                }

                return MapToResponseDto(prodotto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nella cancellazione dell'immagine {imgId} del prodotto {prodottoId} utente {userId}");
                throw;
            }
            ;
        }
    }
}
