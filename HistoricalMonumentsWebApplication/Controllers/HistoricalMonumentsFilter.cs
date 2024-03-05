using HistoricalMonumentsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace HistoricalMonumentsWebApplication.Controllers;

public class HistoricalMonumentsFilter : Controller
{
    private const int PageItems = 8;
    private readonly DblibraryContext _context;

    public HistoricalMonumentsFilter(DblibraryContext context)
    {
        _context = context;
    }

    [Route("historical-monuments/{category?}")]
    public async Task<IActionResult> Index([FromQuery] string categoryOption, [FromRoute] string? category = "all",
        [FromQuery] int page = 1)
    {
        ViewBag.Cities = await _context.Cities.Select(city => city.Name).Distinct().ToListAsync();
        ViewBag.Classifications = await _context.Classifications.Select(classification => classification.Name).Distinct().ToListAsync();

        Dictionary<string, Func<HistoricalMonument, bool>> _filters = new()
        {
            ["classification"] = historicalMonument => historicalMonument.Classification.Name.ToLower() == categoryOption.ToLower(),
            ["city"] = historicalMonument => historicalMonument.City.Name.ToLower() == categoryOption.ToLower()
        };


        var allMonuments = await _context.HistoricalMonuments.Include(h => h.City)
            .Include(h => h.Classification)
            .Include(h => h.Status).ToListAsync();

        var totalElements = allMonuments.Count;

        var pages = (int)Math.Ceiling((decimal)totalElements / PageItems);


        if (page > pages || page < 1 || string.IsNullOrEmpty(category)) return RedirectToAction(nameof(Index), new { page = 1, category = "all" });

        List<HistoricalMonument> filteredMonuments = category == "all" ? allMonuments : Filter(allMonuments, _filters[category]);

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

    private List<HistoricalMonument> Filter(List<HistoricalMonument> allMonuments, Func<HistoricalMonument, bool> predicate)
    {
        return allMonuments.Where(predicate).ToList();
    }
}