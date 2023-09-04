using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Administrator.Controllers
{
    public class CountriesController : Controller
    {
        private readonly RwaMoviesContext _context;


        public CountriesController(RwaMoviesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;
            var Countries = await _context.Countries.ToListAsync();

            return View(Countries.ToPagedList(pageNumber, pageSize));

        }
    }
}
