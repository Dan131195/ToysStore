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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ------- Immagini Porodotto -------

        // POST: api/prodotto/{id}/immagini
        [HttpPost("{id}/immagini")]
        [Authorize]

    }
}