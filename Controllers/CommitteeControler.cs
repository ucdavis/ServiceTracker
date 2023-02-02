using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceTracker.Helpers;
using ServiceTracker.Models;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class CommitteeController : Controller
    {
        private readonly ServiceTrackerContext _context;

        public CommitteeController(ServiceTrackerContext context)
        {
            _context = context;
        }
             

        public async Task<IActionResult> Index()
        {
            var model = await _context.Committees.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id, int year)
        {
            if(year == 0)
            {
                year = YearFinder.Year;
            }
            var model = await _context.Committees.Include(c => c.Members.Where(m=> m.Year == year)).ThenInclude(m => m.Employee).Where(c => c.Id == id).FirstOrDefaultAsync();
            return View(model);
        }

       

        
    }
}