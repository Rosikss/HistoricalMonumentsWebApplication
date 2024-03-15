using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HistoricalMonumentsWebApplication.Models;
using HistoricalMonumentsWebApplication.Services;

namespace HistoricalMonumentsWebApplication.Controllers
{
    public class HistoricalMonumentsController : Controller
    {
        private readonly DblibraryContext _context;
        private readonly IDataPortServiceFactory<HistoricalMonument> _portServiceFactory;
        public HistoricalMonumentsController(DblibraryContext context)
        {
            _context = context;
            _portServiceFactory = new HistoricalMonumentDataPortServiceFactory(_context);
        }

        // GET: HistoricalMonuments
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.HistoricalMonuments.Include(h => h.City).Include(h => h.Classification).Include(h => h.Status);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: HistoricalMonuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonument = await _context.HistoricalMonuments
                .Include(h => h.City)
                .Include(h => h.Classification)
                .Include(h => h.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historicalMonument == null)
            {
                return NotFound();
            }

            return View(historicalMonument);
        }

        // GET: HistoricalMonuments/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            return View();
        }

        // POST: HistoricalMonuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartingYear,EndingYear,Description,CityId,ClassificationId,StatusId")] HistoricalMonument historicalMonument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historicalMonument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", historicalMonument.CityId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Name", historicalMonument.ClassificationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", historicalMonument.StatusId);
            return View(historicalMonument);
        }

        // GET: HistoricalMonuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonument = await _context.HistoricalMonuments.FindAsync(id);
            if (historicalMonument == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", historicalMonument.CityId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Name", historicalMonument.ClassificationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", historicalMonument.StatusId);
            return View(historicalMonument);
        }

        // POST: HistoricalMonuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartingYear,EndingYear,Description,CityId,ClassificationId,StatusId, Photo")] HistoricalMonument historicalMonument)
        {
            if (id != historicalMonument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historicalMonument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoricalMonumentExists(historicalMonument.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", historicalMonument.CityId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Name", historicalMonument.ClassificationId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", historicalMonument.StatusId);
            return View(historicalMonument);
        }

        // GET: HistoricalMonuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonument = await _context.HistoricalMonuments
                .Include(h => h.City)
                .Include(h => h.Classification)
                .Include(h => h.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historicalMonument == null)
            {
                return NotFound();
            }

            return View(historicalMonument);
        }

        // POST: HistoricalMonuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historicalMonument = await _context.HistoricalMonuments.FindAsync(id);
            if (historicalMonument != null)
            {
                _context.HistoricalMonuments.Remove(historicalMonument);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoricalMonumentExists(int id)
        {
            return _context.HistoricalMonuments.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken = default)
        {
            var importService = _portServiceFactory.GetImportService(fileExcel.ContentType);
            await using var stream = fileExcel.OpenReadStream();
            await importService.ImportFromStreamAsync(stream, cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
