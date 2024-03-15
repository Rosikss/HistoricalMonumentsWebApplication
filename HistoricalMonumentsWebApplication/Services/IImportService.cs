using HistoricalMonumentsWebApplication.Models;

namespace HistoricalMonumentsWebApplication.Services
{
    public interface IImportService<TEntity>
        where TEntity : Entity
    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }

}
