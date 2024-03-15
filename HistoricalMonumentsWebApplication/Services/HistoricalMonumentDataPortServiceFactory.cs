using HistoricalMonumentsWebApplication.Models;

namespace HistoricalMonumentsWebApplication.Services
{
    public class HistoricalMonumentDataPortServiceFactory : IDataPortServiceFactory<HistoricalMonument>
    {
        private readonly DblibraryContext _context;
        public HistoricalMonumentDataPortServiceFactory(DblibraryContext context)
        {
            _context = context;
        }
        public IImportService<HistoricalMonument> GetImportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new HistoricalMonumentImportService(_context);
            }
            throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
        }
        public IExportService<HistoricalMonument> GetExportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new HistoricalMonumentExportService(_context);
            }
            throw new NotImplementedException($"No export service implemented for movies with content type {contentType}");
        }
    }

}
