using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DVDMarket.Data;
using DVDMarket.Models;
using Microsoft.AspNetCore.Identity;

namespace DVDMarket.Controllers
{
    public class DVDCleanerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public DVDCleanerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private IQueryable<DVDModel> GetModelsNoMine()
        {
            var currentUserId = _userManager.GetUserId(User);
            var currentEmail = _userManager.GetUserName(User);
            var modelIds = (from us in _context.UserModel
                            where us.UserEmail == currentEmail
                            select us.DvdId);
            var models = from s in _context.DVDModel.Include(p => p.UserModels)
                             //where modelIds.Contains(s.Id)
                         select s;
            return models;
        }
        // GET: DVDCleaner
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var models = GetModelsNoMine();
            
            if (!String.IsNullOrEmpty(searchString))
            {
                models = _context.DVDModel.Where(s => s.Actor.Contains(searchString));
            }
            return View(await _context.DVDModel.ToListAsync());
        }

        // GET: DVDCleaner/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDModel = await _context.DVDModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dVDModel == null)
            {
                return NotFound();
            }

            return View(dVDModel);
        }

        // GET: DVDCleaner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DVDCleaner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url,Title,Actor,Producer,Studio,Cast,Master,Copies,NonpermAge,Released")] DVDModel dVDModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dVDModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dVDModel);
        }

        // GET: DVDCleaner/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDModel = await _context.DVDModel.FindAsync(id);
            if (dVDModel == null)
            {
                return NotFound();
            }
            return View(dVDModel);
        }

        // POST: DVDCleaner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Url,Title,Actor,Producer,Studio,Cast,Master,Copies,NonpermAge,Released")] DVDModel dVDModel)
        {
            if (id != dVDModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dVDModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DVDModelExists(dVDModel.Id))
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
            return View(dVDModel);
        }

        // GET: DVDCleaner/Delete/5
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: MemberManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberModel = await _context.MemberModel.FindAsync(id);
            _context.MemberModel.Remove(memberModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: DVDCleaner/DeleteAllYear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllYear()
        {
            var today = DateTime.Today;
            var dvdModels = _context.DVDModel;

            foreach (var dvdmodel in dvdModels)
            {
                if (today.Subtract(dvdmodel.Released).TotalDays > 365)
                {
                    _context.DVDModel.Remove(dvdmodel);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool DVDModelExists(long id)
        {
            return _context.DVDModel.Any(e => e.Id == id);
        }
    }
}
