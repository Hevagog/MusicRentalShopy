using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Models;
using MvcStore.Data;

namespace MusicRentStore.Controllers
{
    public class UserController : Controller
    {
        private readonly StoreContext _context;

        public UserController(StoreContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var user = _context.User.Include(p=>p.RentHistory).AsNoTracking();
            return View(await user.ToListAsync());
            //   return _context.User != null ? 
            //               View(await _context.User.ToListAsync()) :
            //               Problem("Entity set 'StoreContext.User'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(p=>p.RentHistory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        private void PopulateRentHistoryDropDownList(object selectedRentHistory = null)
        {
            var selectedRentHistories = from e in _context.RentHistory
                                orderby e.AlbumTitle
                                select e;
            var res = selectedRentHistories.AsNoTracking();
            ViewBag.RentHistoriesID = new SelectList(res, "Id", "AlbumTitle", selectedRentHistory);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            PopulateRentHistoryDropDownList();
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName")] User user,IFormCollection form)
        {
            string rentHistoryvalue = form["RentHistory"].ToString();

            if (ModelState.IsValid)
            {
                RentHistory rentHistory = null;
                if(rentHistoryvalue != "-1")
                {
                    var ee = _context.RentHistory.Where(e=>e.Id == int.Parse(rentHistoryvalue));
                    if(ee.Count() > 0)
                        rentHistory = ee.First();
                }
                user.RentHistory = rentHistory;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            // var user = await _context.User.FindAsync(id);
            var user = _context.User.Where(p=>p.Id == id)
                .Include(p=>p.RentHistory)
                .First();
            if (user == null)
            {
                return NotFound();
            }
            if(user.RentHistory != null)
            {
                PopulateRentHistoryDropDownList(user.RentHistory.Id);
            }
            else
            {
                PopulateRentHistoryDropDownList();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName")] User user,IFormCollection form)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String rentHistoryvalue = form["RentHistory"];
                    RentHistory rentHistory = null;
                    if(rentHistoryvalue != "-1")
                    {
                        var ee = _context.RentHistory.Where(e => e.Id == int.Parse(rentHistoryvalue));
                        if (ee.Count() > 0)
                            rentHistory = ee.First();
                    }
                    user.RentHistory = rentHistory;

                    // _context.Update(user);
                    User pp = _context.User.Where(p=>p.Id == id)
                        .Include(p=>p.RentHistory)
                        .First();
                    pp.RentHistory = rentHistory;
                    pp.UserName = user.UserName;
                    pp.Stores = user.Stores;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            // var user = await _context.User
            //     .FirstOrDefaultAsync(m => m.Id == id);
            var user = _context.User.Where(p=>p.Id == id)
                .Include(p=>p.RentHistory)
                .First();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'StoreContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
