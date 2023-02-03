using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceTracker.Helpers;
using ServiceTracker.Models;
using System.Text.Encodings.Web;
using ServiceTracker.Controllers;

namespace MvcMovie.Controllers
{
    public class CommitteeController : SuperController
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
            var model = await CommitteeDetailsViewModel.Create(_context, id, year);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {           
            var model = await _context.Committees.Where(c => c.Id == id).FirstOrDefaultAsync();
            return View(model);
        }

       

        
    }
}