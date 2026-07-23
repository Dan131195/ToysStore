using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToysStore.DTOs;
using ToysStore.DTOs.Prodotto;
using ToysStore.Services;

namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdottoController : ControllerBase
    {
        private readonly ProdottoService _prodottoService;

        public ProdottoController(ProdottoService prodottoService)
        {
            _prodottoService = prodottoService;
        }

        // GET: api/prodotto 
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProdottoResponseDto>>> GetAll()
        {
            try
            {
                var prodotti = await _prodottoService.GetAllProdottiAsync();
                return Ok(prodotti);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Errore durante il caricamento dei prodotti." });
            }

        }

        // GET: api/prodotto/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdottoResponseDto>> GetById(Guid id)
        {
            try
            {
                var prodotto = await _prodottoService.GetProdottoByIdAsync(id);
                if (prodotto == null) return NotFound("Prodotto non trovato.");

                return Ok(prodotto);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Errore durante il caricamento del prodotto." });
            }
        }

        // GET: api/prodotto/utente/{userId} 
        [HttpGet("utente/{userId}")]
        public async Task<ActionResult<IEnumerable<ProdottoResponseDto>>> GetByUtente(string userId)
        {
            try
            {
                var prodotti = await _prodottoService.GetProdottiByUserIdAsync(userId);
                return Ok(prodotti);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Errore durante il caricamento dei prodotti." });
            }

        }

        // POST: api/prodotto
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProdottoResponseDto>> Create([FromForm] ProdottoCreateDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var nuovoProdotto = await _prodottoService.CreateProdottoAsync(dto, userId);

                return CreatedAtAction(nameof(GetById), new { id = nuovoProdotto.GiocattoloId }, nuovoProdotto);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Errore durante la creazone del prodotto." });
            }
        }

        // PUT: api/prodotto/{id} 
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProdottoResponseDto>> Update(Guid id, [FromBody] ProdottoUpdateDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                try
                {
                    var prodottoAggiornato = await _prodottoService.UpdateProdottoAsync(id, dto, userId);
                    if (prodottoAggiornato == null) return NotFound("Prodotto non trovato.");

                    return Ok(prodottoAggiornato);
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Forbid(ex.Message);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Errore durante la modifca del prodotto." });
            }

        }

        // DELETE: api/prodotto/{id} 
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var success = await _prodottoService.DeleteProdottoAsync(id, userId);
                if (!success) return BadRequest("Prodotto non trovato o non sei autorizzato a eliminarlo.");

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Errore durante la cancellazione del prodotto." });
            }
        }

        // ------- Immagini Porodotto -------

        // POST: api/prodotto/{id}/immagini
        [HttpPost("{id}/immagini")]
        [Authorize]
        public async Task<IActionResult> AddImmagini(Guid id, [FromForm] IEnumerable<IFormFile> img)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                if(img == null || !img.Any())
                {
                    return BadRequest("Nessuna immagine selezionata.");
                }

                var newProdotto = _prodottoService.AddImmagineProdottoAsync(id, img, userId);

                if (newProdotto == null) return NotFound("Prodotto non trovato");

                return Ok(newProdotto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Errore durante la cancellazione dell'immagine del prodotto." });
            }
        }

        // DELETE: api/prodotto/{prodottoId}/immagini/{immagineId}
        [HttpDelete("{prodottoId}/immagini/{immagineId}")]
        [Authorize]
        public async Task<IActionResult> DeleteImmagine(Guid prodottoId, Guid ImmagineId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var newProdotto = _prodottoService.DeleteImmagineProdotto(prodottoId, ImmagineId, userId);

                if (newProdotto == null) return NotFound("Immagine del prodtto non trovata.");

                return Ok(newProdotto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Si è verificato un errore durante l'eliminazione dell'immagine.");
            }
        }
    }
}