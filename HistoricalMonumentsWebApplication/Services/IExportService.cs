using HistoricalMonumentsWebApplication.Models.Entities;

namespace HistoricalMonumentsWebApplication.Services
{
    public interface IExportService<TEntity> where TEntity : Entity
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }
}
