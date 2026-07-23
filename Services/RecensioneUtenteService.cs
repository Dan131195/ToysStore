using ToysStore.Data;

namespace ToysStore.Services
{
    public class RecensioneUtenteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RecensioneUtenteService> _logger;
        
        public RecensioneUtenteService(ApplicationDbContext context, ILogger<RecensioneUtenteService> logger)
        {
            _context = context;
            _logger = logger;
        }


    }
}
