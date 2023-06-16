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
    public class StoreController : Controller
    {
        private readonly StoreContext _context;

        public StoreController(StoreContext context)
        {
            _context = context;
        }

        // GET: Store
        public async Task<IActionResult> Index()
        {
            var store = _context.Store.Include(p => p.User).Include(p=>p.Album).AsNoTracking();
            return View(await store.ToListAsync());
            //   return _context.Store != null ? 
            //               View(await _context.Store.ToListAsync()) :
            //               Problem("Entity set 'StoreContext.Store'  is null.");
        }

        // GET: Store/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Store == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .Include(p=> p.User)
                .Include(p=>p.Album)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        private void PopulateUserDropDownList(object selectedUser = null)
        {
            var selectedUsers = from e in _context.User
                                orderby e.UserName
                                select e;
            var res = selectedUsers.AsNoTracking();
            ViewBag.UsersID = new SelectList(res, "Id", "UserName", selectedUser);
        }

        private void PopulateAlbumDropDownList(object selectedAlbum = null)
        {
            var selectedAlbums = from e in _context.Album
                                orderby e.Name
                                select e;
            var res = selectedAlbums.AsNoTracking();
            ViewBag.AlbumsID = new SelectList(res, "Id", "Name", selectedAlbum);
        }

        // GET: Store/Create
        public IActionResult Create()
        {
            PopulateAlbumDropDownList();
            PopulateUserDropDownList();
            return View();
        }

        // POST: Store/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Store store, IFormCollection form)
        {
            string albumValue = form["Album"].ToString();
            string userValue = form["User"].ToString();
            if (ModelState.IsValid)
            {
                Album album = null;
                if(albumValue != "-1")
                {
                    var ee = _context.Album.Where(e=>e.Id == int.Parse(albumValue));
                    if(ee.Count() > 0)
                        album = ee.First();
                }
                User user = null;
                if(userValue != "-1")
                {
                    var ee = _context.User.Where(e=>e.Id == int.Parse(userValue));
                    if(ee.Count() > 0)
                        user = ee.First();
                }
                store.Album = album;
                store.User = user;

                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Store/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Store == null)
            {
                return NotFound();
            }

            // var store = await _context.Store.FindAsync(id);
            var store = _context.Store.Where(p=>p.Id == id)
                .Include(p=>p.Album)
                .Include(p=>p.User)
                .First();
            if (store == null)
            {
                return NotFound();
            }
            if(store.Album != null)
            {
                PopulateAlbumDropDownList(store.Album.Id);
            }
            else
            {
                PopulateAlbumDropDownList();
            }
            if(store.User != null)
            {
                PopulateUserDropDownList(store.User.Id);
            }
            else
            {
                PopulateUserDropDownList();
            }
            return View(store);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Store store,IFormCollection form)
        {
            if (id != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String albumValue = form["Album"];
                    String userValue = form["User"];
                    Album album = null;
                    if(albumValue != "-1")
                    {
                        var ee = _context.Album.Where(e=>e.Id == int.Parse(albumValue));
                        if(ee.Count()>0)
                            album = ee.First();
                    }
                    User user = null;
                    if(userValue != "-1")
                    {
                        var ee = _context.User.Where(e=>e.Id == int.Parse(userValue));
                        if(ee.Count()>0)
                            user = ee.First();
                    }
                    store.Album = album;
                    store.User = user;
                    // _context.Update(store);
                    Store pp = _context.Store.Where(p=> p.Id == id)
                        .Include(p=>p.Album)
                        .Include(p=>p.User)
                        .First();
                    pp.Album = album;
                    pp.User = user;
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.Id))
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
            return View(store);
        }

        // GET: Store/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Store == null)
            {
                return NotFound();
            }

            // var store = await _context.Store
            //     .FirstOrDefaultAsync(m => m.Id == id);
            var store = _context.Store.Where(p=>p.Id == id)
                .Include(p=>p.Album)
                .Include(p=>p.User)
                .First();
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Store == null)
            {
                return Problem("Entity set 'StoreContext.Store'  is null.");
            }
            var store = await _context.Store.FindAsync(id);
            if (store != null)
            {
                _context.Store.Remove(store);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
          return (_context.Store?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
