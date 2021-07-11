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
    public class DVDLoanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DVDLoanController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
                         //where modelIds.Contains(s.Id)
                         select s;

            return models;
        }

        // GET: DVDLoan
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
        
        // GET: DVDLoan/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_context.MemberModel.Count() == 0)
            {
                return NotFound();
            }

            var dVDModel = await _context.DVDModel.FindAsync(id);

            ViewData["DVDPermission"] = dVDModel.NonpermAge.ToString();

            if (dVDModel == null)
            {
                return NotFound();
            }

            return View(dVDModel);
        }

        // POST: DVDLoan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Url,Title,Actor,Producer,Studio,Cast,Master,Copies,Released")] DVDModel dVDModel,
                                        DateTime BorrowDate, DateTime ExceptDate, string MemberEmail)
        {
            if (id != dVDModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    long modelid = 0;
                    DateTime now = System.DateTime.Now;
                    modelid = now.Ticks / 100;

                    LoanModel loanModel = new LoanModel();
                    loanModel.LoanId = modelid;
                    loanModel.BorrowDate = BorrowDate;
                    loanModel.ExceptDate = ExceptDate;
                    loanModel.BorrowType = true;
                    loanModel.DvdId = dVDModel.Id;
                    loanModel.Dvd = dVDModel;
                    loanModel.MemberEmail = MemberEmail;

                    _context.Add(loanModel);

                    dVDModel.Master = true;
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

        // GET: DVDLoan/Delete/5
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

        // POST: DVDLoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, [Bind("Id,Url,Title,Actor,Producer,Studio,Cast,Master,Copies,Released")] DVDModel dVDModel,
                            DateTime ReceiveDate)
        {
            if (id != dVDModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var loan = _context.LoanModel.Where(u => u.DvdId == id && u.BorrowType == true);

                    loan.First().ReceiveDate = ReceiveDate;
                    loan.First().BorrowType = false;
                    _context.LoanModel.Update(loan.First());
                    await _context.SaveChangesAsync();

                    dVDModel.Master = false;
                    
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
        
        private bool DVDModelExists(long id)
        {
            return _context.DVDModel.Any(e => e.Id == id);
        }
    }
}
