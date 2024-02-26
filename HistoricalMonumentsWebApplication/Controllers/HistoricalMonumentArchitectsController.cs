using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HistoricalMonumentsWebApplication.Models;

namespace HistoricalMonumentsWebApplication.Controllers
{
    public class HistoricalMonumentArchitectsController : Controller
    {
        private readonly DblibraryContext _context;

        public HistoricalMonumentArchitectsController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: HistoricalMonumentArchitects
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.HistoricalMonumentArchitects.Include(h => h.Architect).Include(h => h.HistoricalMonument);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: HistoricalMonumentArchitects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonumentArchitect = await _context.HistoricalMonumentArchitects
                .Include(h => h.Architect)
                .Include(h => h.HistoricalMonument)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historicalMonumentArchitect == null)
            {
                return NotFound();
            }

            return View(historicalMonumentArchitect);
        }

        // GET: HistoricalMonumentArchitects/Create
        public IActionResult Create()
        {
            ViewData["ArchitectId"] = new SelectList(_context.Architects, "Id", "LastName");
            ViewData["HistoricalMonumentId"] = new SelectList(_context.HistoricalMonuments, "Id", "Name");
            return View();
        }

        // POST: HistoricalMonumentArchitects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArchitectId,HistoricalMonumentId")] HistoricalMonumentArchitect historicalMonumentArchitect)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historicalMonumentArchitect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArchitectId"] = new SelectList(_context.Architects, "Id", "Id", historicalMonumentArchitect.ArchitectId);
            ViewData["HistoricalMonumentId"] = new SelectList(_context.HistoricalMonuments, "Id", "Id", historicalMonumentArchitect.HistoricalMonumentId);
            return View(historicalMonumentArchitect);
        }

        // GET: HistoricalMonumentArchitects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonumentArchitect = await _context.HistoricalMonumentArchitects.FindAsync(id);
            if (historicalMonumentArchitect == null)
            {
                return NotFound();
            }
            ViewData["ArchitectId"] = new SelectList(_context.Architects, "Id", "LastName", historicalMonumentArchitect.ArchitectId);
            ViewData["HistoricalMonumentId"] = new SelectList(_context.HistoricalMonuments, "Id", "Name", historicalMonumentArchitect.HistoricalMonumentId);
            return View(historicalMonumentArchitect);
        }

        // POST: HistoricalMonumentArchitects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArchitectId,HistoricalMonumentId")] HistoricalMonumentArchitect historicalMonumentArchitect)
        {
            if (id != historicalMonumentArchitect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historicalMonumentArchitect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoricalMonumentArchitectExists(historicalMonumentArchitect.Id))
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
            ViewData["ArchitectId"] = new SelectList(_context.Architects, "Id", "LastName", historicalMonumentArchitect.ArchitectId);
            ViewData["HistoricalMonumentId"] = new SelectList(_context.HistoricalMonuments, "Id", "Name", historicalMonumentArchitect.HistoricalMonumentId);
            return View(historicalMonumentArchitect);
        }

        // GET: HistoricalMonumentArchitects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonumentArchitect = await _context.HistoricalMonumentArchitects
                .Include(h => h.Architect)
                .Include(h => h.HistoricalMonument)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historicalMonumentArchitect == null)
            {
                return NotFound();
            }

            return View(historicalMonumentArchitect);
        }

        // POST: HistoricalMonumentArchitects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historicalMonumentArchitect = await _context.HistoricalMonumentArchitects.FindAsync(id);
            if (historicalMonumentArchitect != null)
            {
                _context.HistoricalMonumentArchitects.Remove(historicalMonumentArchitect);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoricalMonumentArchitectExists(int id)
        {
            return _context.HistoricalMonumentArchitects.Any(e => e.Id == id);
        }
    }
}
