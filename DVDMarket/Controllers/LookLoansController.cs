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
    public class LookLoansController : Controller
    {
        private readonly ApplicationDbContext _context;
        static List<DateTime> _queryDate = new List<DateTime>();
        private readonly UserManager<IdentityUser> _userManager;

        public LookLoansController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private IQueryable<LoanModel> GetModelsNoMine()
        {
            var currentUserId = _userManager.GetUserId(User);
            var currentEmail = _userManager.GetUserName(User);
            var modelIds = (from us in _context.UserModel
                            where us.UserEmail == currentEmail
                            select us.DvdId);


            var models = from s in _context.LoanModel.Include(p => p.UserModels)
                             //where !modelIds.Contains(s.Id)
                         select s;

            return models;
        }

        // GET: LookLoans
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = _context.LoanModel.Include(l => l.Dvd);

            var tempDay = "";

            foreach (var model in applicationDbContext)
            {
                if (tempDay != model.BorrowDate.ToShortDateString())
                {
                    _queryDate.Add(model.BorrowDate);
                    tempDay = model.BorrowDate.ToShortDateString();
                }
            }
            
            if (_queryDate.Count() > 0)
            {
                ViewData["TotalCount"] = GetTotalOutCount(_queryDate[0]);
            }
            ViewData["CurrentFilter"] = searchString;
            var movies = from m in _context.LoanModel
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.MemberEmail.Contains(searchString));
            }


            return View(await movies.ToListAsync());
        }

        // GET: LookLoans/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanModel = await _context.LoanModel
                .Include(l => l.Dvd)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanModel == null)
            {
                return NotFound();
            }
            
            return View(loanModel);
        }

        // GET: LookLoans/Create
        public IActionResult Create()
        {
            ViewData["DvdId"] = new SelectList(_context.DVDModel, "Id", "Id");
            return View();
        }

        // POST: LookLoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LoanId,BorrowType,BorrowDate,ExceptDate,ReceiveDate,DvdId")] LoanModel loanModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DvdId"] = new SelectList(_context.DVDModel, "Id", "Id", loanModel.DvdId);
            return View(loanModel);
        }
        
        // GET: LookLoans/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanModel = await _context.LoanModel.FindAsync(id);
            if (loanModel == null)
            {
                return NotFound();
            }
            ViewData["DvdId"] = new SelectList(_context.DVDModel, "Id", "Id", loanModel.DvdId);
            return View(loanModel);
        }

        // POST: LookLoans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,LoanId,BorrowType,BorrowDate,ExceptDate,ReceiveDate,DvdId")] LoanModel loanModel)
        {
            if (id != loanModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanModelExists(loanModel.Id))
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
            ViewData["DvdId"] = new SelectList(_context.DVDModel, "Id", "Id", loanModel.DvdId);
            return View(loanModel);
        }

        // GET: LookLoans/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanModel = await _context.LoanModel
                .Include(l => l.Dvd)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanModel == null)
            {
                return NotFound();
            }

            return View(loanModel);
        }

        // POST: LookLoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var loanModel = await _context.LoanModel.FindAsync(id);
            _context.LoanModel.Remove(loanModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: LookLoans/GetTotalCount/Date
        [HttpPost]
        [ValidateAntiForgeryToken]
        public int GetTotalCount(long id)
        {
            return GetTotalOutCount(_queryDate[(int)id]);
        }

        private int GetTotalOutCount(DateTime date)
        {
            return _context.LoanModel.Where(l => l.BorrowDate == date).Count();
        }

        private bool LoanModelExists(long id)
        {
            return _context.LoanModel.Any(e => e.Id == id);
        }
    }
}
