using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Administrator.ViewModels;
using DAL.Models;
using X.PagedList;
using AutoMapper;

namespace Administrator.Controllers
{
    public class VideosController : Controller
    {
        private readonly RwaMoviesContext _context;
        private readonly IMapper _mapper;

        public VideosController(RwaMoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Videos
        public async Task<IActionResult> Index(int? page, string? searchText)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;
            List<Video> rwaMoviesContextPaged = null;
            if (searchText != null) {
                rwaMoviesContextPaged =
                    await _context.Videos
                    .Include(v => v.Genre)
                    .Include(v => v.Image)
                    .Where(v => v.Name.Contains(searchText) || v.Genre.Name.Contains(searchText))
                    .OrderBy(v => v.CreatedAt)
                    .ToListAsync();

                ViewData["searchText"] = searchText;
                return View(rwaMoviesContextPaged.ToPagedList(pageNumber, pageSize));
            }
                rwaMoviesContextPaged =
                    await _context.Videos
                    .Include(v => v.Genre)
                    .Include(v => v.Image)
                    .OrderBy(v => v.CreatedAt)
                    .ToListAsync();

            return View(rwaMoviesContextPaged.ToPagedList(pageNumber, pageSize));
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var video = await _context.Videos
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Videos/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Content");
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMVideo video)
        {
            var DALvideo = _mapper.Map<Video>(video);
            if (ModelState.IsValid)
            {
                _context.Add(DALvideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", DALvideo.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", DALvideo.ImageId);
            return View(video);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", video.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Content", video.ImageId);
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, VMVideo video)
        {
            var DALvideo = _mapper.Map<Video>(video);
            if (id != DALvideo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DALvideo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(DALvideo.Id))
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
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", DALvideo.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", DALvideo.ImageId);
            return View(DALvideo);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Videos == null)
            {
                return Problem("Entity set 'RwaMoviesContext.Videos'  is null.");
            }
            var video = await _context.Videos.FindAsync(id);
            if (video != null)
            {
                _context.Videos.Remove(video);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoExists(int? id)
        {
            return (_context.Videos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
