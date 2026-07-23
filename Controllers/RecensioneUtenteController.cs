using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToysStore.Services;
namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecensioneUtenteController : ControllerBase
    {
        private readonly RecensioneUtenteService _recensioneProdottoService;

        public RecensioneUtenteController(RecensioneUtenteService recesnioneProdottoervice)
        {
            _recensioneProdottoService = recesnioneProdottoervice;
        }
    }
}
