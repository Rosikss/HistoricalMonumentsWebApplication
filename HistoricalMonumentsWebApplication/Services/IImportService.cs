using HistoricalMonumentsWebApplication.Models.Entities;

namespace HistoricalMonumentsWebApplication.Services
{
    public interface IImportService<TEntity>
        where TEntity : Entity
    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }

}
