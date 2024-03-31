using HistoricalMonumentsWebApplication.Filters.ActionFilters;
using HistoricalMonumentsWebApplication.Models;
using HistoricalMonumentsWebApplication.Models.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HistoricalMonumentsWebApplication.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DblibraryContext _context;


        public HomeController(ILogger<HomeController> logger, DblibraryContext context)
        {
            _logger = logger;
            _context = context;
        }

        [TypeFilter(typeof(AddCountriesActionFilters))]
        [Route("Home")]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var historicalMonuments = await _context.HistoricalMonuments.ToListAsync();

            var lastThreeHistoricalMonuments = historicalMonuments.TakeLast(3).ToList();

            return View(lastThreeHistoricalMonuments);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult>
            GetCountByCountry(CancellationToken cancellationToken)
        {
            var allMonuments = await _context.HistoricalMonuments.Include(h => h.City)
                .Include(h => h.Classification)
                .Include(h => h.Status)
                .Include(h => h.City).ThenInclude(c => c.Country).ToListAsync(cancellationToken);

            var translations = new Dictionary<string, string>()
            {
                ["Україна"] = "Ukraine",
                ["Німеччина"] = "Germany",
                ["Франція"] = "France",
                ["Італія"] = "Italy",
                ["Чехія"] = "CZ",
            };

            foreach (var monument in allMonuments)
            {
                if (translations.TryGetValue(monument.City.Country.Name, out string translatedCountry))
                {
                    monument.City.Country.Name = translatedCountry;
                }
            }

            var responseItems = allMonuments
                .GroupBy(h => h.City.Country.Name)
                .Select(group => new CountByCountryItem(group.Key.ToString(), group.Count()))
                .ToList();

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
