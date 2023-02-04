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

        public async Task<IActionResult> Details(int id, int ViewYear)
        {
            var model = await CommitteeDetailsViewModel.Create(_context, id, ViewYear);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {           
            var model = await _context.Committees.Where(c => c.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Committee not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Committees commitee)
        {
            var commiteeToUpdate = await _context.Committees.Where(c => c.Id == commitee.Id).FirstOrDefaultAsync();
            if(commiteeToUpdate == null)
            {
                ErrorMessage = "Committee not found!";
                return RedirectToAction(nameof(Index));
            }
            commiteeToUpdate.Name = commitee.Name;
            commiteeToUpdate.Description = commitee.Description;
            commiteeToUpdate.Term = commitee.Term;

            if(ModelState.IsValid)
            {
                await _context.SaveChangesAsync();
                Message = "Committee updated";
                return RedirectToAction(nameof(Details), new { id = commitee.Id }); 
            }
            return View(commitee);
        }

       

        
    }
}