using ToysStore.Data;

namespace ToysStore.Services
{
    public class RecensioneProdottoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RecensioneProdottoService> _logger;
        
        public RecensioneProdottoService(ApplicationDbContext context, ILogger<RecensioneProdottoService> logger)
        {
            _context = context;
            _logger = logger;
        }


    }
}
