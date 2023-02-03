using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceTracker.Models;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        private readonly ServiceTrackerContext _context;

        public HelloWorldController(ServiceTrackerContext context)
        {
            _context = context;
        }

        

        public async Task<IActionResult> Index()
        {
            var model = await _context.Employees.Where(e => e.Id == "10205674").FirstOrDefaultAsync();
            return View(model);
        }

        public async Task<IActionResult> Committees()
        {
            var model = await _context.Committees.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> CommitteeMembers(int id)
        {
            var model = await _context.Committees.Include(c => c.Members).ThenInclude(m => m.Employee).Where(c => c.Id == id).FirstOrDefaultAsync();
            return View(model);
        }

        public ActionResult YearList()
        {
            var model = ServiceTracker.Helpers.YearFinder.YearList;
            return View(model);
        }

        // 
        // GET: /HelloWorld/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        
    }
}