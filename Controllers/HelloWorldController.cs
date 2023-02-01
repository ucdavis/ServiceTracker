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

        // 
        // GET: /HelloWorld/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        
    }
}