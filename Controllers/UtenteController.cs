using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToysStore.Services;

namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtenteController : ControllerBase
    {
        private readonly UtenteService _utenteService;
        private readonly ILogger<UtenteController> _logger;

        public UtenteController(UtenteService utenteService, ILogger<UtenteController> logger)
        {
            _utenteService = utenteService;
            _logger = logger;
        }

    }
}
