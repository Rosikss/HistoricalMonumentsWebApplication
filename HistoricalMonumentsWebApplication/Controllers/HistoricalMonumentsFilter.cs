using HistoricalMonumentsWebApplication.Models;
using HistoricalMonumentsWebApplication.Models.DbContexts;
using HistoricalMonumentsWebApplication.Models.Entities;
using HistoricalMonumentsWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Authorization;


namespace HistoricalMonumentsWebApplication.Controllers;
[AllowAnonymous]
public class HistoricalMonumentsFilter : Controller
{
    private const int PageItems = 8;
    private readonly DblibraryContext _context;
    private readonly IDataPortServiceFactory<HistoricalMonument> _portServiceFactory;

    public HistoricalMonumentsFilter(DblibraryContext context)
    {
        _context = context;
        _portServiceFactory = new HistoricalMonumentDataPortServiceFactory(_context);
    }

    [Route("historical-monuments/{category?}")]
    public async Task<IActionResult> Index([FromQuery] string categoryOption, string? searchString, [FromRoute] string? category = "all",
        [FromQuery] int page = 1)
    {
        var allMonuments = await _context.HistoricalMonuments.Include(h => h.City)
            .Include(h => h.Classification)
            .Include(h => h.Status)
            .Include(h => h.City).ThenInclude(c => c.Country).ToListAsync();

        ViewBag.Cities = await _context.Cities.Select(city => city.Name).Distinct().ToListAsync();
        ViewBag.Classifications = await _context.Classifications.Select(classification => classification.Name).Distinct().ToListAsync();
        ViewBag.Countries = await _context.Countries.Select(classification => classification.Name).Distinct().ToListAsync();

        categoryOption = WebUtility.HtmlDecode(categoryOption);

        Dictionary<string, Func<HistoricalMonument, bool>> _filters = new()
        {
            ["classification"] = historicalMonument => historicalMonument.Classification.Name.ToLower() == categoryOption.ToLower(),
            ["city"] = historicalMonument => historicalMonument.City.Name.ToLower() == categoryOption.ToLower(),
            ["country"] = historicalMonument => historicalMonument.City.Country.Name.ToLower() == categoryOption.ToLower(),
        };

        var totalElements = allMonuments.Count;

        var pages = (int)Math.Ceiling((decimal)totalElements / PageItems);

        if (page > pages || page < 1 || string.IsNullOrEmpty(category)) return RedirectToAction(nameof(Index), new { page = 1, category = "all" });

        List<HistoricalMonument> filteredMonuments = category == "all" ? allMonuments : Filter(allMonuments, _filters[category]);

        if (!string.IsNullOrEmpty(searchString))
        {
            var searchMonuments =
                allMonuments.Where(historicalMonument => historicalMonument.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            
            totalElements = searchMonuments.Count;

            pages = (int)Math.Ceiling((decimal)totalElements / PageItems);
            var paginationModel2 = new PaginationModel
            {
                HistoricalMonuments = searchMonuments,
                Pages = pages,
                CurrentPage = page,
                Category = category,
                CategoryOption = string.IsNullOrEmpty(categoryOption) ? "Empty" : categoryOption
            };

            return View(paginationModel2);
        }

        var paginationHistoricalMonuments = filteredMonuments.Skip((page - 1) * PageItems).Take(PageItems).ToList();

        totalElements = filteredMonuments.Count;

        pages = (int)Math.Ceiling((decimal)totalElements / PageItems);

        var paginationModel = new PaginationModel
        {
            HistoricalMonuments = paginationHistoricalMonuments,
            Pages = pages,
            CurrentPage = page,
            Category = category,
            CategoryOption = string.IsNullOrEmpty(categoryOption)  ? "Empty" : categoryOption
        };

        return View(paginationModel);
    }

    [HttpGet]
    public async Task<IActionResult> Export([FromQuery] string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        CancellationToken cancellationToken = default)
    {
        var exportService = _portServiceFactory.GetExportService(contentType);

        var memoryStream = new MemoryStream();

        await exportService.WriteToAsync(memoryStream, cancellationToken);

        await memoryStream.FlushAsync(cancellationToken);
        memoryStream.Position = 0;


        return new FileStreamResult(memoryStream, contentType)
        {
            FileDownloadName = $"historical_monument_{DateTime.UtcNow.ToShortDateString()}.xlsx"
        };
    }

    private List<HistoricalMonument> Filter(List<HistoricalMonument> allMonuments, Func<HistoricalMonument, bool> predicate)
    {
        return allMonuments.Where(predicate).ToList();
    }
}