using HistoricalMonumentsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HistoricalMonumentsWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DblibraryContext _context;


        public HomeController(ILogger<HomeController> logger, DblibraryContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var historicalMonuments = await _context.HistoricalMonuments.ToListAsync();

            var lastThreeHistoricalMonuments = historicalMonuments.TakeLast(3).ToList();

            return View(lastThreeHistoricalMonuments);
        }

        [HttpGet("countByCategory")]
        public async Task<IActionResult>
            GetCountByCategoryAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context.HistoricalMonuments
                .GroupBy(historicalMonument => historicalMonument.Classification.Name)
                .Select(group => new CountByCategoryItem(group.Key.ToString(), group.Count()))
                .ToListAsync(cancellationToken: cancellationToken);
                
            return Json(responseItems);
        }

        [HttpGet("countByCategory2")]
        public async Task<IActionResult>
            GetCountByYearAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context.HistoricalMonumentMaterials
                .GroupBy(historicalMonument => historicalMonument.Material.Name)
                .Select(group => new CountByCategoryItem(group.Key.ToString(), group.Count()))
                .ToListAsync(cancellationToken: cancellationToken);

            return Json(responseItems);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
