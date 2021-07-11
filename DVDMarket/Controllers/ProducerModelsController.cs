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
    public class ProducerModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProducerModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProducerModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProducerModel.ToListAsync());
        }

        // GET: ProducerModels/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producerModel = await _context.ProducerModel
                .FirstOrDefaultAsync(m => m.ProducerId == id);
            if (producerModel == null)
            {
                return NotFound();
            }

            return View(producerModel);
        }

        // GET: ProducerModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProducerModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProducerId,Name,Gender,DOB")] ProducerModel producerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producerModel);
        }

        // GET: ProducerModels/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producerModel = await _context.ProducerModel.FindAsync(id);
            if (producerModel == null)
            {
                return NotFound();
            }
            return View(producerModel);
        }

        // POST: ProducerModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProducerId,Name,Gender,DOB")] ProducerModel producerModel)
        {
            if (id != producerModel.ProducerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducerModelExists(producerModel.ProducerId))
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
            return View(producerModel);
        }

        // GET: ProducerModels/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producerModel = await _context.ProducerModel
                .FirstOrDefaultAsync(m => m.ProducerId == id);
            if (producerModel == null)
            {
                return NotFound();
            }

            return View(producerModel);
        }

        // POST: ProducerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var producerModel = await _context.ProducerModel.FindAsync(id);
            _context.ProducerModel.Remove(producerModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducerModelExists(long id)
        {
            return _context.ProducerModel.Any(e => e.ProducerId == id);
        }
    }
}
