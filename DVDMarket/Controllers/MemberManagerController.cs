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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DVDMarket.Controllers
{
    public class MemberManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        List<MemberModel> myModels;
        static string sortOrder = "";

        private void GetUserManager(ApplicationDbContext context, string sortorder = "")
        {
            var users = context.Users.Select(s=>s);

            if (users == null)
            {
                return;
            }
           
            if (sortorder == "")
            {
                sortorder = sortOrder;
            }
            ViewData["SearchUserName"] = string.IsNullOrEmpty(sortorder) ? "username_desc" : "";

            switch (sortorder)
            {
                case "username_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                default:
                    users = users.OrderBy(s => s.UserName);
                    break;
            }
            sortOrder = sortorder;

            myModels = new List<MemberModel>(context.Users.Count());

            var listIndex = 0;
            foreach (var user in users)
            {
                MemberModel model = new MemberModel();
                model.MemberName = user.UserName;
                model.MemberEmail = user.Email;
                model.MemberStockId = listIndex;
                if (context.MemberModel.Where(m => m.MemberEmail == user.Email).SingleOrDefault() != null)
                {
                    var member = context.MemberModel.Where(m => m.MemberEmail == user.Email).SingleOrDefault();
                    model.Permission = member.Permission;
                    model.Category = member.Category;
                }
                else
                {
                    model.Permission = "";
                }
                
                myModels.Add(model);
                listIndex++;
            }
            //}
        }

        public MemberManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MemberManager
        public IActionResult Index(string sortOrder)
        {
            GetUserManager(_context, sortOrder);

            if (myModels == null)
            {
                return NotFound();
            }
            
            return View(myModels);
        }

        // GET: MemberManager/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GetUserManager(_context, sortOrder);

            var model = myModels.Where(m => m.MemberStockId == id).First();

            return View(model);
        }

        // POST: MemberManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberStockId,MemberName,MemberEmail")] MemberModel memberModel,
                                string MemberType, string MemberCategory)
        {
            if (id < 0)
            {
                return NotFound();
            }

            GetUserManager(_context, sortOrder);
            if (ModelState.IsValid)
            {
                memberModel.MemberName = myModels.Where(m => m.MemberStockId == id).First().MemberName;
                memberModel.MemberEmail = myModels.Where(m => m.MemberStockId == id).First().MemberEmail;
                memberModel.Permission = MemberType;
                memberModel.Category = MemberCategory;
                memberModel.MemberStockId = 0;
                _context.Add(memberModel);
                await _context.SaveChangesAsync();

                myModels.Where(m => m.MemberStockId == id).First().Permission = MemberType;
                return RedirectToAction(nameof(Index));
            }
            return View(memberModel);
        }

        // GET: MemberManager/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GetUserManager(_context, sortOrder);

            var model = myModels.Where(m => m.MemberStockId == id).First();

            return View(model);
        }

        // POST: MemberManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GetUserManager(_context, sortOrder);
            var email = myModels.Where(m => m.MemberStockId == id).First().MemberEmail;
            var index = _context.MemberModel.Where(m => m.MemberEmail == email).First().MemberStockId;
            var memberModel = await _context.MemberModel.FindAsync(index);
            _context.MemberModel.Remove(memberModel);
            await _context.SaveChangesAsync();
            myModels.Where(m => m.MemberStockId == id).First().Permission = "";
            return RedirectToAction(nameof(Index));
        }

        private bool MemberModelExists(int id)
        {
            return _context.MemberModel.Any(e => e.MemberStockId == id);
        }
    }
}
