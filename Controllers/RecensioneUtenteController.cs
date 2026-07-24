using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToysStore.DTOs.RecensioneProdotto;
using ToysStore.Services;
namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecensioneUtenteController : ControllerBase
    {
        private readonly RecensioneUtenteService _recensioneUtenteService;

        public RecensioneUtenteController(RecensioneUtenteService recensioneUtenteService)
        {
            _recensioneUtenteService = recensioneUtenteService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> OttieniRecensioniVenditore(Guid userId)
        {
            var recensioni = await _recensioneUtenteService.GetAllRecensioniUtenteAsync(userId);

            if (recensioni == null || !recensioni.Any())
                return NotFound("Nessuna recensione trovata per questo venditore.");

            return Ok(recensioni);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecensione([FromBody] CreateRecensioneUtenteDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Utente non valido");

                var success = await _recensioneUtenteService.AddRecensioneUtenteAsync(dto, userId);
                if (!success) return BadRequest("NOn è possibile fare una recensione a qesto ordine.");

                return Ok();
            }
            catch (Exception)
            {
                {
                    return StatusCode(500, new { Message = "Errore durante la creazione della recensione." });
                }
            }
        }

        [HttpPut("{recensioneId}")]
        public async Task<IActionResult> UpdateRecensioneUtente([FromBody] UpdateRecensioneUtenteDto dto, Guid recensioneId, Guid UtenteId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                try
                {

                }
            }
            catch
            {
                return StatusCode(500, new { Message = "Errore durante la modifica della recensione" });
            }
        }
    }
}
