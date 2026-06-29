using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToysStore.Services;
using ToysStore.DTOs.IndirizzoUtente;

namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IndirizzoUtenteController : ControllerBase
    {
        private readonly UtenteService _utenteService;

        public IndirizzoUtenteController(UtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        // GET: api/Indirizzo/MieiIndirizzi
        [HttpGet("MieiIndirizzi")]
        public async Task<IActionResult> GetMieiIndirizzi()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var indirizzi = await _utenteService.GetIndirizziByUserIdAsync(userId);
            return Ok(indirizzi);
        }

        // POST: api/Indirizzo/Aggiungi
        [HttpPost("Aggiungi")]
        public async Task<IActionResult> AggiungiIndirizzo([FromBody] IndirizzoUtenteCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var risultato = await _utenteService.AddIndirizzoAsync(userId, dto);
            if (risultato == null) return NotFound(new { Message = "Utente non trovato." });

            return StatusCode(201, risultato); // 201 Created
        }

        // PUT: api/Indirizzo/Modifica/{id}
        [HttpPut("Modifica/{id}")]
        public async Task<IActionResult> ModificaIndirizzo(Guid id, [FromBody] IndirizzoUtenteUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var success = await _utenteService.UpdateIndirizzoAsync(userId, id, dto);
            if (!success) return NotFound(new { Message = "Indirizzo non trovato o non appartenente a questo utente." });

            return Ok(new { Message = "Indirizzo aggiornato con successo!" });
        }

        // DELETE: api/Indirizzo/Elimina/{id}
        [HttpDelete("Elimina/{id}")]
        public async Task<IActionResult> EliminaIndirizzo(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var success = await _utenteService.DeleteIndirizzoAsync(userId, id);
            if (!success) return NotFound(new { Message = "Indirizzo non trovato o impossibile da eliminare." });

            return Ok(new { Message = "Indirizzo rimosso con successo." });
        }
    }
}
