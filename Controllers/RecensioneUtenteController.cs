using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToysStore.DTOs.RecensioneProdotto;
using ToysStore.Services;
namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecensioneUtenteController : ControllerBase
    {
        private readonly RecensioneUtenteService _recensioneUtenteService;

        public RecensioneUtenteController(RecensioneUtenteService recensioneUtenteService)
        {
            _recensioneUtenteService = recensioneUtenteService;
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRecensioniUtente(Guid userId)
        {
            var recensioni = await _recensioneUtenteService.GetAllRecensioniUtenteAsync(userId);

            if (recensioni == null || !recensioni.Any())
                return NotFound("Nessuna recensione trovata per questo venditore.");

            return Ok(recensioni);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecensione([FromBody] RecensioneUtenteCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Utente non valido");

                var success = await _recensioneUtenteService.AddRecensioneUtenteAsync(dto, userId);
                if (success == null) return BadRequest("Non è possibile fare una recensione a questo ordine.");

                return Ok();
            }
            catch (Exception)
            {
                {
                    return StatusCode(500, new { Message = "Errore durante la creazione della recensione." });
                }
            }
        }

        [HttpPut("{recensioneId}/{utenteId}")]
        public async Task<IActionResult> UpdateRecensioneUtente([FromBody] RecensioneUtenteUpdateDto dto, Guid recensioneId, Guid utenteId)
        {
            try
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(user)) return Unauthorized();

                var recensioneUpdated = await _recensioneUtenteService.UpdateRecensioneUtenteAsync(dto, utenteId, recensioneId);
                if (recensioneUpdated == null) return NotFound("Recensione Utente non trovata.");

                return Ok(recensioneUpdated);
            }
            catch
            {
                return StatusCode(500, new { Message = "Errore durante la modifica della recensione" });
            }
        }
        /*
        [HttpDelete("{userId}/{recensioneId}")]
        public async Task<IActionResult> DeleteRecensioneUtente(Guid userId, Guid recensioneId) { 
            
        }*/
    }
}
