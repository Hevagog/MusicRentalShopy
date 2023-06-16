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
    public class ArtistController : Controller
    {
        private readonly StoreContext _context;

        public ArtistController(StoreContext context)
        {
            _context = context;
        }

        // GET: Artist
        public async Task<IActionResult> Index()
        {
            var artist = _context.Artist.Include(p=>p.BandMember).AsNoTracking();
            return View(await artist.ToListAsync());
            //   return _context.Artist != null ? 
            //               View(await _context.Artist.ToListAsync()) :
            //               Problem("Entity set 'StoreContext.Artist'  is null.");
        }

        // GET: Artist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(p=>p.BandMember)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        private void PopulateBandMembersDropDownList(object selectedBandMember = null)
        {
            var selectedBandMembers = from e in _context.BandMember
                                orderby e.Name
                                select e;
            var res = selectedBandMembers.AsNoTracking();
            ViewBag.BandMembersID = new SelectList(res, "Id", "Name", selectedBandMember);
        }

        // GET: Artist/Create
        public IActionResult Create()
        {
            PopulateBandMembersDropDownList();
            return View();
        }

        // POST: Artist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country,DateOfFormation")] Artist artist,IFormCollection form)
        {
            string bandMemberValue = form["BandMember"].ToString();
            if (ModelState.IsValid)
            {
                BandMember bandMember = null;
                if(bandMemberValue != "-1")
                {
                    var ee = _context.BandMember.Where(e => e.Id == int.Parse(bandMemberValue));
                    if (ee.Count() > 0)
                        bandMember = ee.First();
                }
                artist.BandMember = bandMember;

                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            // var artist = await _context.Artist.FindAsync(id);
            var artist = _context.Artist.Where(p=> p.Id == id)
                .Include(p=>p.BandMember)
                .First();
            if (artist == null)
            {
                return NotFound();
            }
            if(artist.BandMember != null)
            {
                PopulateBandMembersDropDownList(artist.BandMember.Id);
            }
            else
            {
                PopulateBandMembersDropDownList();
            }
            return View(artist);
        }

        // POST: Artist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country,DateOfFormation")] Artist artist,IFormCollection form)
        {
            if (id != artist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String bandMemberValue = form["BandMember"];
                    BandMember bandMember = null;
                    if(bandMemberValue != "-1")
                    {
                        var ee = _context.BandMember.Where(e => e.Id == int.Parse(bandMemberValue));
                        if (ee.Count() > 0)
                            bandMember = ee.First();
                    }
                    artist.BandMember = bandMember;

                    // _context.Update(artist);
                    Artist pp = _context.Artist.Where(p=>p.Id == id)
                        .Include(p=>p.BandMember)
                        .First();
                    pp.BandMember = bandMember;
                    pp.Albums = artist.Albums;
                    pp.Country = artist.Country;
                    pp.DateOfFormation = artist.DateOfFormation;
                    pp.Name = artist.Name;
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.Id))
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
            return View(artist);
        }

        // GET: Artist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            // var artist = await _context.Artist
            //     .FirstOrDefaultAsync(m => m.Id == id);
            var artist = _context.Artist.Where(p=> p.Id == id)
                .Include(p=>p.BandMember)
                .First();
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artist == null)
            {
                return Problem("Entity set 'StoreContext.Artist'  is null.");
            }
            var artist = await _context.Artist.FindAsync(id);
            if (artist != null)
            {
                _context.Artist.Remove(artist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
          return (_context.Artist?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
