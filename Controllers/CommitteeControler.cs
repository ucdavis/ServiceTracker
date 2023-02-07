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

        public async Task<IActionResult> AddMember(int id)
        {
            var model = await CommitteMemberAddViewModel.Create(_context, id, 0);
            if(model.committee == null)
            {
                ErrorMessage = "Committee not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> GetMembers(string idType)
        {
            if(idType == "Faculty")
            {
                var faculty = await _context.Employees.Where(e => e.VoteCategory != 0).OrderBy(e => e.LastName).ThenBy(e => e.LastName).Select(e => new GenericMember{ Id = e.Id, FirstName = e.FirstName, LastName = e.LastName}).ToListAsync();
                return Json(faculty); 
            }
            if(idType == "Manual Members")
            {
                var members = await _context.Members.OrderBy(m => m.LastName).ThenBy(m => m.FirstName).Select(m => new GenericMember{ Id = m.Id.ToString(), FirstName = m.FirstName, LastName = m.LastName}).ToListAsync();
                return Json(members);
            }
            if(idType == "Admin Staff")
            {
                var staff = await _context.Employees.Where(e => e.AdminStaff && e.Current).OrderBy(e => e.LastName).ThenBy(e => e.LastName).Select(e => new GenericMember{ Id = e.Id, FirstName = e.FirstName, LastName = e.LastName}).ToListAsync();
                return Json(staff);
            }
            if(idType == "All Non-faculty")
            {
                var everyone = await _context.Employees.Where(e => e.Current && e.VoteCategory == 0).OrderBy(e => e.LastName).ThenBy(e => e.LastName).Select(e => new GenericMember{ Id = e.Id, FirstName = e.FirstName, LastName = e.LastName}).ToListAsync();
                return Json(everyone);
            }
            return BadRequest();
        }

       

        
    }
}