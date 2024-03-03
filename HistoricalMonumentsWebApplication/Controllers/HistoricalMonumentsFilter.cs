using HistoricalMonumentsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HistoricalMonumentsWebApplication.Controllers
{
    public class HistoricalMonumentsFilter : Controller
    {
        private readonly DblibraryContext _context;
        private const int PageItems = 6;

        public HistoricalMonumentsFilter(DblibraryContext context)
        {
            _context = context;
        }
        [Route("historical-monuments")]
        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {

            

            int totalElements = await _context.HistoricalMonuments.CountAsync();

            int pages = (int)Math.Ceiling((decimal)totalElements / PageItems);
            
            if (page > pages || page < 1)
            {
                return RedirectToAction(nameof(Index), new { page = 1 });
            }

            var countries = await _context.Countries.ToListAsync();

            ViewBag.Countries = countries;

            var paginationHistoricalMonuments = await _context.HistoricalMonuments.Skip((int)((page - 1) * PageItems)).Take(PageItems).ToListAsync();
            
            PaginationModel paginationModel2 = new PaginationModel
            {
                HistoricalMonuments = paginationHistoricalMonuments,
                Pages = pages,
                
            };
            return View(paginationModel2);

        }
    }
}
