using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
