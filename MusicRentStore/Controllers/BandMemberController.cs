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
    public class BandMemberController : Controller
    {
        private readonly StoreContext _context;

        public BandMemberController(StoreContext context)
        {
            _context = context;
        }

        // GET: BandMember
        public async Task<IActionResult> Index()
        {
              return _context.BandMember != null ? 
                          View(await _context.BandMember.ToListAsync()) :
                          Problem("Entity set 'StoreContext.BandMember'  is null.");
        }

        // GET: BandMember/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BandMember == null)
            {
                return NotFound();
            }

            var bandMember = await _context.BandMember
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bandMember == null)
            {
                return NotFound();
            }

            return View(bandMember);
        }

        // GET: BandMember/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BandMember/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Instrument,Birthday")] BandMember bandMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bandMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bandMember);
        }

        // GET: BandMember/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BandMember == null)
            {
                return NotFound();
            }

            var bandMember = await _context.BandMember.FindAsync(id);
            if (bandMember == null)
            {
                return NotFound();
            }
            return View(bandMember);
        }

        // POST: BandMember/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Instrument,Birthday")] BandMember bandMember)
        {
            if (id != bandMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bandMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BandMemberExists(bandMember.Id))
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
            return View(bandMember);
        }

        // GET: BandMember/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BandMember == null)
            {
                return NotFound();
            }

            var bandMember = await _context.BandMember
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bandMember == null)
            {
                return NotFound();
            }

            return View(bandMember);
        }

        // POST: BandMember/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BandMember == null)
            {
                return Problem("Entity set 'StoreContext.BandMember'  is null.");
            }
            var bandMember = await _context.BandMember.FindAsync(id);
            if (bandMember != null)
            {
                _context.BandMember.Remove(bandMember);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BandMemberExists(int id)
        {
          return (_context.BandMember?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
