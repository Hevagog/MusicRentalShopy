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
    public class RentHistoryController : Controller
    {
        private readonly StoreContext _context;

        public RentHistoryController(StoreContext context)
        {
            _context = context;
        }

        // GET: RentHistory
        public async Task<IActionResult> Index()
        {
              return _context.RentHistory != null ? 
                          View(await _context.RentHistory.ToListAsync()) :
                          Problem("Entity set 'StoreContext.RentHistory'  is null.");
        }

        // GET: RentHistory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RentHistory == null)
            {
                return NotFound();
            }

            var rentHistory = await _context.RentHistory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentHistory == null)
            {
                return NotFound();
            }

            return View(rentHistory);
        }

        // GET: RentHistory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RentHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AlbumTitle,DateOfRent,DateOfReturn")] RentHistory rentHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rentHistory);
        }

        // GET: RentHistory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RentHistory == null)
            {
                return NotFound();
            }

            var rentHistory = await _context.RentHistory.FindAsync(id);
            if (rentHistory == null)
            {
                return NotFound();
            }
            return View(rentHistory);
        }

        // POST: RentHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AlbumTitle,DateOfRent,DateOfReturn")] RentHistory rentHistory)
        {
            if (id != rentHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentHistoryExists(rentHistory.Id))
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
            return View(rentHistory);
        }

        // GET: RentHistory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RentHistory == null)
            {
                return NotFound();
            }

            var rentHistory = await _context.RentHistory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentHistory == null)
            {
                return NotFound();
            }

            return View(rentHistory);
        }

        // POST: RentHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RentHistory == null)
            {
                return Problem("Entity set 'StoreContext.RentHistory'  is null.");
            }
            var rentHistory = await _context.RentHistory.FindAsync(id);
            if (rentHistory != null)
            {
                _context.RentHistory.Remove(rentHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentHistoryExists(int id)
        {
          return (_context.RentHistory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
