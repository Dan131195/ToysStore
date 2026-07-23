using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToysStore.Services;
namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecensioneProdottoController : ControllerBase
    {
        private readonly RecensioneProdottoService _recensioneProdottoervice;

        public RecensioneProdottoController(RecensioneProdottoService recesnioneProdottoervice)
        {
            _recensioneProdottoervice = recesnioneProdottoervice;
        }
    }
}
