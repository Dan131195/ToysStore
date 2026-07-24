using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToysStore.DTOs;
using ToysStore.Services;

namespace ToysStore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdiniController : ControllerBase
    {
        private readonly OrdineService _ordineService;

        public OrdiniController(OrdineService ordineService)
        {
            _ordineService = ordineService;
        }

        [HttpPost("crea-test")]
        public async Task<IActionResult> CreaOrdineTest([FromBody] CreaOrdineDto dto)
        {
            // Estrae l'ID utente (stringa) dal token di chi fa la chiamata
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Devi essere loggato per creare un ordine.");

            var ordineId = await _ordineService.CreaOrdineTestAsync(dto, userId);

            // Restituisce l'ID dell'ordine appena creato, ti servirà da passare alla POST della recensione
            return Ok(new
            {
                Messaggio = "Ordine finto creato con successo (Stato: Ricevuto). Pronto per essere recensito!",
                OrdineId = ordineId
            });
        }
    }
}