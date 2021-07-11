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
    public class DVDShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DVDShopController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [NonAction]
        private IQueryable<DVDModel> GetModelsNoMine()
        {
            var currentUserId = _userManager.GetUserId(User);
            var currentEmail = _userManager.GetUserName(User);
            var modelIds = (from us in _context.UserModel
                            where us.UserEmail == currentEmail
                            select us.DvdId);


            var models = from s in _context.DVDModel.Include(p => p.UserModels)
                             //where !modelIds.Contains(s.Id)
                         select s;

            return models;
        }

        // GET: DVDShop
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {

            ViewData["ReleasedSortParam"] = string.IsNullOrEmpty(sortOrder) ? "released_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var models = GetModelsNoMine();
            
            if (!String.IsNullOrEmpty(searchString))
            {
                models = models.Where(s => s.Actor.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "released_desc":
                    models = models.OrderByDescending(s => s.Released);
                    break;
                default:
                    models = models.OrderBy(s => s.Released);
                    break;
            }

            return View(await models.AsNoTracking().ToListAsync());
        }

        // GET: DVDShop/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDModel = await _context.DVDModel
                .FirstOrDefaultAsync(m => m.Id == id);

            string[] Sub = dVDModel.Actor.Split(',');
            ViewData["Actor_Count"] = Sub.Length + " Members";
            if (!dVDModel.NonpermAge)
            {
                ViewData["PermissionAge"] = "Permission";
            }
            else
            {
                ViewData["PermissionAge"] = "Nonpermission";
            }

            if (dVDModel == null)
            {
                return NotFound();
            }

            return View(dVDModel);
        }

        // GET: DVDShop/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DVDShop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url,Title,Actor,Producer,Studio,Cast,Master,Copies,Released,NonpermAge")] DVDModel dVDModel)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = _userManager.GetUserId(User);
                var currentUserEmail = _userManager.GetUserName(User);

                var uModel = _context.UserModel.Include(s => s.Dvd).Where(u => u.DvdId == dVDModel.Id && u.UserEmail == currentUserEmail);
                var cUser = _context.Users.Find(currentUserId);

                if (uModel.Any())
                {
                    uModel.First().UserId = currentUserId;
                    _context.UserModel.Update(uModel.First());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    UserModel userStock = new UserModel()
                    {
                        DvdId = dVDModel.Id,
                        UserId = currentUserId,
                        Dvd = dVDModel,
                        User = cUser,
                        UserEmail = cUser.Email
                    };
                    _context.Add(userStock);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

            return View(dVDModel);
        }

        // GET: DVDShop/Edit/5
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

        // POST: DVDShop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Url,Title,Actor,Producer,Studio,Cast,Master,Copies,Released,NonpermAge")] DVDModel dVDModel)
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

        // GET: DVDShop/Delete/5
        public async Task<IActionResult> Delete(long? id)
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

        // POST: DVDShop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var dVDModel = await _context.DVDModel.FindAsync(id);
            _context.DVDModel.Remove(dVDModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DVDModelExists(long id)
        {
            return _context.DVDModel.Any(e => e.Id == id);
        }
    }
}
