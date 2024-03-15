using ClosedXML.Excel;
using HistoricalMonumentsWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace HistoricalMonumentsWebApplication.Services
{
    public class HistoricalMonumentExportService : IExportService<HistoricalMonument>
    {
        private const string RootWorksheetName = "Historical Monuments";

        private static readonly IReadOnlyList<string> HeaderNames =
            new string[]
            {
                "Назва",
                "Початок будівництва",
                "Кінець будівництва",
                "Опис",
                "Місто",
                "Категорія",
                "Статус",
            };

        private readonly DblibraryContext _context;

        public HistoricalMonumentExportService(DblibraryContext context)
        {
            _context = context;
        }

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private void WriteHistoricalMonument(IXLWorksheet worksheet, HistoricalMonument monument, int rowIndex)
        {
            var columnIndex = 1;
            worksheet.Cell(rowIndex, columnIndex++).Value = monument.Name;
            worksheet.Cell(rowIndex, columnIndex++).Value = monument.StartingYear?.ToString("yyyy-MM-dd");
            worksheet.Cell(rowIndex, columnIndex++).Value = monument.EndingYear?.ToString("yyyy-MM-dd");
            worksheet.Cell(rowIndex, columnIndex++).Value = monument.Description;
            worksheet.Cell(rowIndex, columnIndex++).Value = monument.City?.Name;
            worksheet.Cell(rowIndex, columnIndex++).Value = monument.Classification?.Name;
            worksheet.Cell(rowIndex, columnIndex++).Value = monument.Status?.Name;
        }

        private void WriteHistoricalMonuments(IXLWorksheet worksheet, ICollection<HistoricalMonument> monuments)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var monument in monuments)
            {
                WriteHistoricalMonument(worksheet, monument, rowIndex);
                rowIndex++;
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var monuments = await _context.HistoricalMonuments
                .Include(m => m.City)
                .Include(m => m.Classification)
                .Include(m => m.Status)
                .ToListAsync(cancellationToken);

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(RootWorksheetName);

            WriteHistoricalMonuments(worksheet, monuments);

            workbook.SaveAs(stream);
        }
    }
}
