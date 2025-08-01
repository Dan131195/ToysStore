using ToysStore.Data;

namespace ToysStore.Services
{
    public class ProdottoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProdottoService> _logger;

        public ProdottoService(ApplicationDbContext context, ILogger<ProdottoService> logger)
        {
            _context = context;
            _logger = logger;
        }

    }
}
