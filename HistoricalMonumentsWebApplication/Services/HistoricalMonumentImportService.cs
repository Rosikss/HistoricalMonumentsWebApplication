using ClosedXML.Excel;
using HistoricalMonumentsWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace HistoricalMonumentsWebApplication.Services
{
    public class HistoricalMonumentImportService : IImportService<HistoricalMonument>
    {
        private readonly DblibraryContext _context;

        public HistoricalMonumentImportService(DblibraryContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Data cannot be read", nameof(stream));
            }

            using (XLWorkbook workBook = new XLWorkbook(stream))
            {
                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                {
                    // Assuming the worksheet contains historical monument data
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        await AddHistoricalMonumentAsync(row, cancellationToken);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddHistoricalMonumentAsync(IXLRow row, CancellationToken cancellationToken)
        {
            HistoricalMonument monument = new HistoricalMonument();
            monument.Name = GetCellValue(row, 1);
            monument.StartingYear = ParseDate(GetCellValue(row, 2));
            monument.EndingYear = ParseDate(GetCellValue(row, 3));
            monument.Description = GetCellValue(row, 4);

            // Assuming City, Classification, and Status are retrieved from database based on their names
            monument.City = await _context.Cities.FirstOrDefaultAsync(c => c.Name == GetCellValue(row, 5), cancellationToken);
            monument.Classification = await _context.Classifications.FirstOrDefaultAsync(c => c.Name == GetCellValue(row, 6), cancellationToken);
            monument.Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == GetCellValue(row, 7), cancellationToken);

            _context.HistoricalMonuments.Add(monument);
        }

        private string GetCellValue(IXLRow row, int columnIndex)
        {
            return row.Cell(columnIndex).Value.ToString();
        }

        private DateTime? ParseDate(string dateValue)
        {
            if (DateTime.TryParse(dateValue, out DateTime result))
            {
                return result.Date;
            }
            return null;
        }
    }
}
