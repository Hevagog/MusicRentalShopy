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
    public class AlbumController : Controller
    {
        private readonly StoreContext _context;

        public AlbumController(StoreContext context)
        {
            _context = context;
        }

        // GET: Album
        public async Task<IActionResult> Index()
        {
            var album = _context.Album.Include(p =>p.Artist).AsNoTracking();
            return View(await album.ToListAsync());
            //   return _context.Album != null ? 
            //               View(await _context.Album.ToListAsync()) :
            //               Problem("Entity set 'StoreContext.Album'  is null.");
        }

        // GET: Album/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Album == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(p=>p.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        private void PopulateArtistsDropDownList(object selectedArtist = null)
        {
            var selectedArtists = from e in _context.Artist
                                orderby e.Name
                                select e;
            var res = selectedArtists.AsNoTracking();
            ViewBag.ArtistsID = new SelectList(res, "Id", "Name", selectedArtist);
        }

        // GET: Album/Create
        public IActionResult Create()
        {
            PopulateArtistsDropDownList();
            return View();
        }

        // POST: Album/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,capacity,Name,ReleaseDate,Genre")] Album album, IFormCollection form)
        {
            string artistValue = form["Artist"].ToString();
            if (ModelState.IsValid)
            {
                Artist artist = null;
                if(artistValue != "-1")
                {
                    var ee = _context.Artist.Where(e => e.Id == int.Parse(artistValue));
                    if (ee.Count() > 0)
                        artist = ee.First();
                }
                album.Artist = artist;

                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Album/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Album == null)
            {
                return NotFound();
            }

            // var album = await _context.Album.FindAsync(id);
            var album = _context.Album
                            .Include(p=>p.Artist)
                            .First();
            if (album == null)
            {
                return NotFound();
            }
            if(album.Artist != null)
            {
                PopulateArtistsDropDownList(album.Artist.Id);
            }
            else
            {
                PopulateArtistsDropDownList();
            }
            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,capacity,Name,ReleaseDate,Genre")] Album album, IFormCollection form)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String artistValue = form["Artist"];
                    Artist artist = null;

                    if(artistValue !="-1")
                    {
                        var ee = _context.Artist.Where(e => e.Id == int.Parse(artistValue));
                        if (ee.Count() > 0)
                            artist = ee.First();
                    }
                    album.Artist = artist;

                    // _context.Update(album);
                    Album pp  = _context.Album.Where(p=>p.Id == id)
                        .Include(p=>p.Artist)
                        .First();
                    pp.Artist = artist;
                    pp.capacity = album.capacity;
                    pp.Genre = album.Genre;
                    pp.Name = album.Name;
                    pp.ReleaseDate = album.ReleaseDate;
                    pp.Stores = album.Stores;
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
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
            return View(album);
        }

        // GET: Album/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Album == null)
            {
                return NotFound();
            }
            var album = _context.Album
                        .Include(p=>p.Artist)
                        .First();
            // var album = await _context.Album
            //     .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Album == null)
            {
                return Problem("Entity set 'StoreContext.Album'  is null.");
            }
            var album = await _context.Album.FindAsync(id);
            if (album != null)
            {
                _context.Album.Remove(album);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
          return (_context.Album?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
