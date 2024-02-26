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
    public class ArchitectsController : Controller
    {
        private readonly DblibraryContext _context;

        public ArchitectsController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Architects
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.Architects.Include(a => a.Country);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: Architects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var architect = await _context.Architects
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (architect == null)
            {
                return NotFound();
            }

            return View(architect);
        }

        // GET: Architects/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        // POST: Architects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,CountryId,BirthYear,DeathYear")] Architect architect)
        {
            if (ModelState.IsValid)
            {
                _context.Add(architect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", architect.CountryId);
            return View(architect);
        }

        // GET: Architects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var architect = await _context.Architects.FindAsync(id);
            if (architect == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", architect.CountryId);
            return View(architect);
        }

        // POST: Architects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,CountryId,BirthYear,DeathYear")] Architect architect)
        {
            if (id != architect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(architect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArchitectExists(architect.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", architect.CountryId);
            return View(architect);
        }

        // GET: Architects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var architect = await _context.Architects
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (architect == null)
            {
                return NotFound();
            }

            return View(architect);
        }

        // POST: Architects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var architect = await _context.Architects.FindAsync(id);
            if (architect != null)
            {
                _context.Architects.Remove(architect);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArchitectExists(int id)
        {
            return _context.Architects.Any(e => e.Id == id);
        }
    }
}
