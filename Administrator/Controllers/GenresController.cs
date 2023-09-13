using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using AutoMapper;
using Administrator.ViewModels;

namespace Administrator.Controllers
{
    public class GenresController : Controller
    {
        private readonly RwaMoviesContext _context;
        private readonly IMapper _mapper;
        public GenresController(RwaMoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
              return _context.Genres != null ? 
                          View(await _context.Genres.ToListAsync()) :
                          Problem("Entity set 'RwaMoviesContext.Genres'  is null.");
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            var vmGenres = _mapper.Map<VMGenre>(genre);
            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMGenre genre)
        {
            var DALGenre = _mapper.Map<Genre>(genre);
            if (ModelState.IsValid)
            {
                _context.Add(DALGenre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(DALGenre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VMGenre genre)
        {
            var DALGenre = _mapper.Map<Genre>(genre);

            if (id != DALGenre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DALGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(DALGenre.Id))
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
            return View(DALGenre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'RwaMoviesContext.Genres'  is null.");
            }

    
            var hasDependentVideos = await _context.Videos.AnyAsync(v => v.GenreId == id);
            if (hasDependentVideos)
            {
                return BadRequest("Cannot delete this genre because it is referenced by videos.");
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Genre not found." });
        }

        private bool GenreExists(int id)
        {
          return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult GenreIndexTableBody()
        {
            var genres = _context.Genres.ToListAsync();
            return PartialView("_GenreIndexTableBody", genres);
        }
    }
}
