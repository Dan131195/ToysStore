using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToysStore.DTOs.Utente;
using ToysStore.Services;

namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtenteController : ControllerBase
    {
        private readonly UtenteService _utenteService;

        public UtenteController(UtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        // GET: api/Utente/MioProfilo 
        [HttpGet("MioProfilo")]
        public async Task<IActionResult> GetUtente()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized(new { Message = "Token non valido." });

                var utente = await _utenteService.GetUtenteByUserIdAsync(userId);
                if (utente == null) return NotFound(new { Message = "Profilo non trovato." });

                var response = new UtenteResponseDto
                {
                    Nome = utente.User.FirstName,
                    Cognome = utente.User.LastName,
                    Email = utente.User.Email,
                    Nickname = utente.Nickname,

                    ImmagineProfiloUrl = !string.IsNullOrEmpty(utente.ImmagineProfilo)
                        ? $"{Request.Scheme}://{Request.Host}/{utente.ImmagineProfilo}"
                        : null
                };

                return Ok(response);
            }
            catch
            {
                return StatusCode(500, new { Message = "Errore durante il caricamento del profilo." });
            }
        }

        // PUT: api/Utente/UpdateProfilo
        [HttpPut("UpdateProfilo")]
        public async Task<IActionResult> UpdateProfilo([FromForm] UtenteUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized(new { Message = "Token non valido." });

                var success = await _utenteService.UpdateUtenteAsync(userId, dto);

                if (!success)
                {
                    return NotFound(new { Message = "Proilo Utente non trovato." });
                }

                return Ok(new { Message = "Profilo aggiornato con successo!" });
            }
            catch 
            {
                return StatusCode(500, new { Message = "Errore durante la modifica del profilo." });
            }

        }

        // DELETE: api/Utente/DeleteAccount
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { Message = "Token non valido." });

            try
            {
                var result = await _utenteService.DeleteUtenteAsync(userId);

                if (result == null)
                {
                    return NotFound(new { Message = "Utente non trovato nel database." });
                }

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Account eliminato definitivamente con successo." });
                }

                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
            }
            catch (DbUpdateException)
            {

                return BadRequest(new
                {
                    Message = "Impossibile eliminare l'account: hai uno storico di ordini (acquisti o vendite) collegati al tuo profilo. Per motivi fiscali non possiamo eliminare i dati. Contatta l'assistenza."
                });
            }
            catch 
            {
                return StatusCode(500, new { Message = "Errore durante l'eliminazione del profilo." });
            }
        }

    }
}
