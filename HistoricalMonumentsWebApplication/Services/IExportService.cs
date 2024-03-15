using HistoricalMonumentsWebApplication.Models;

namespace HistoricalMonumentsWebApplication.Services
{
    public interface IExportService<TEntity> where TEntity : Entity
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }
}
