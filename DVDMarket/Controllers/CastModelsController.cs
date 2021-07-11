using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DVDMarket.Data;
using DVDMarket.Models;

namespace DVDMarket.Controllers
{
    public class CastModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CastModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CastModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.CastModel.ToListAsync());
        }

        // GET: CastModels/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castModel = await _context.CastModel
                .FirstOrDefaultAsync(m => m.CastId == id);
            if (castModel == null)
            {
                return NotFound();
            }

            return View(castModel);
        }

        // GET: CastModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CastModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CastId,Name,Gender,DOB")] CastModel castModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(castModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(castModel);
        }

        // GET: CastModels/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castModel = await _context.CastModel.FindAsync(id);
            if (castModel == null)
            {
                return NotFound();
            }
            return View(castModel);
        }

        // POST: CastModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CastId,Name,Gender,DOB")] CastModel castModel)
        {
            if (id != castModel.CastId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(castModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CastModelExists(castModel.CastId))
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
            return View(castModel);
        }

        // GET: CastModels/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castModel = await _context.CastModel
                .FirstOrDefaultAsync(m => m.CastId == id);
            if (castModel == null)
            {
                return NotFound();
            }

            return View(castModel);
        }

        // POST: CastModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var castModel = await _context.CastModel.FindAsync(id);
            _context.CastModel.Remove(castModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CastModelExists(long id)
        {
            return _context.CastModel.Any(e => e.CastId == id);
        }
    }
}
