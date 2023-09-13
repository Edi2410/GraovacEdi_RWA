using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Javni.ViewModels;
using DAL.Models;
using X.PagedList;
using AutoMapper;

namespace Javni.Controllers
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
            ViewData["pages"] = pageNumber;
            List<Video> rwaMoviesContextPaged = null;
            if (searchText != null)
            {
                rwaMoviesContextPaged =
                    await _context.Videos
                    .Include(v => v.Genre)
                    .Include(v => v.Image)
                    .Where(v => v.Name.Contains(searchText) || v.Genre.Name.Contains(searchText))
                    .OrderBy(v => v.CreatedAt)
                    .ToListAsync();

                ViewData["pages"] = rwaMoviesContextPaged.Count() / pageSize;
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append("SearchText", searchText, options);
                ViewData["page"] = page;

                return View(rwaMoviesContextPaged.ToPagedList(pageNumber, pageSize));
            }
            rwaMoviesContextPaged =
                await _context.Videos
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .OrderBy(v => v.CreatedAt)
                .ToListAsync();

            Response.Cookies.Delete("SearchText");
            ViewData["pages"] = rwaMoviesContextPaged.Count() / pageSize;
            ViewData["page"] = page;
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

        
    }
}
