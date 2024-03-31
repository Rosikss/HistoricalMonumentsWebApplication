using HistoricalMonumentsWebApplication.Models.DbContexts;
using HistoricalMonumentsWebApplication.Models.Entities;
using HistoricalMonumentsWebApplication.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HistoricalMonumentsWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(UserTypeOptions.Admin))]
    public class HistoricalMonumentMaterialsController : Controller
    {
        private readonly DblibraryContext _context;

        public HistoricalMonumentMaterialsController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: HistoricalMonumentMaterials
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.HistoricalMonumentMaterials.Include(h => h.HistoricalMonument).Include(h => h.Material);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: HistoricalMonumentMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonumentMaterial = await _context.HistoricalMonumentMaterials
                .Include(h => h.HistoricalMonument)
                .Include(h => h.Material)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historicalMonumentMaterial == null)
            {
                return NotFound();
            }

            return View(historicalMonumentMaterial);
        }

        // GET: HistoricalMonumentMaterials/Create
        public IActionResult Create()
        {
            ViewData["HistoricalMonumentId"] = new SelectList(_context.HistoricalMonuments, "Id", "Name");
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name");
            return View();
        }

        // POST: HistoricalMonumentMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HistoricalMonumentId,MaterialId")] HistoricalMonumentMaterial historicalMonumentMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historicalMonumentMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HistoricalMonumentId"] = new SelectList(_context.HistoricalMonuments, "Id", "Name", historicalMonumentMaterial.HistoricalMonumentId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", historicalMonumentMaterial.MaterialId);
            return View(historicalMonumentMaterial);
        }

        // GET: HistoricalMonumentMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonumentMaterial = await _context.HistoricalMonumentMaterials.FindAsync(id);
            if (historicalMonumentMaterial == null)
            {
                return NotFound();
            }
            ViewData["HistoricalMonumentId"] = new SelectList(_context.HistoricalMonuments, "Id", "Name", historicalMonumentMaterial.HistoricalMonumentId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", historicalMonumentMaterial.MaterialId);
            return View(historicalMonumentMaterial);
        }

        // POST: HistoricalMonumentMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HistoricalMonumentId,MaterialId")] HistoricalMonumentMaterial historicalMonumentMaterial)
        {
            if (id != historicalMonumentMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historicalMonumentMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoricalMonumentMaterialExists(historicalMonumentMaterial.Id))
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
            ViewData["HistoricalMonumentId"] = new SelectList(_context.HistoricalMonuments, "Id", "Name", historicalMonumentMaterial.HistoricalMonumentId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", historicalMonumentMaterial.MaterialId);
            return View(historicalMonumentMaterial);
        }

        // GET: HistoricalMonumentMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicalMonumentMaterial = await _context.HistoricalMonumentMaterials
                .Include(h => h.HistoricalMonument)
                .Include(h => h.Material)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historicalMonumentMaterial == null)
            {
                return NotFound();
            }

            return View(historicalMonumentMaterial);
        }

        // POST: HistoricalMonumentMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historicalMonumentMaterial = await _context.HistoricalMonumentMaterials.FindAsync(id);
            if (historicalMonumentMaterial != null)
            {
                _context.HistoricalMonumentMaterials.Remove(historicalMonumentMaterial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoricalMonumentMaterialExists(int id)
        {
            return _context.HistoricalMonumentMaterials.Any(e => e.Id == id);
        }
    }
}
