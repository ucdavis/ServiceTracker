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
        public async Task<IActionResult> Edit(Committees committee)
        {
            var committeeToUpdate = await _context.Committees.Where(c => c.Id == committee.Id).FirstOrDefaultAsync();
            if(committeeToUpdate == null)
            {
                ErrorMessage = "Committee not found!";
                return RedirectToAction(nameof(Index));
            }
            committeeToUpdate.Name = committee.Name;
            committeeToUpdate.Description = committee.Description;
            committeeToUpdate.Term = committee.Term;

            if(ModelState.IsValid)
            {
                await _context.SaveChangesAsync();
                Message = "Committee updated";
                return RedirectToAction(nameof(Details), new { id = committee.Id }); 
            }
            return View(committee);
        }

        public async Task<IActionResult> AddMember(int id)
        {
            var model = await CommitteeMemberAddViewModel.Create(_context, id, 0);
            if(model.committee == null)
            {
                ErrorMessage = "Committee not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(CommitteeMemberAddViewModel vm)
        {
            var newMember = new CommitteeMembers();
            var submittedMember = vm.member;
            var employeeCheck = await _context.Employees.Where(e => e.Id == submittedMember.EmployeeId).AnyAsync();
            if(employeeCheck){
                newMember.EmployeeId = submittedMember.EmployeeId;
            } else {
                newMember.MemberId = submittedMember.MemberId;
            }
            newMember.CommitteeId = submittedMember.CommitteeId;            
            newMember.StartYear = submittedMember.StartYear;
            newMember.EndYear = submittedMember.StartYear + vm.AppointmentLength - 1;
            newMember.Chair = submittedMember.Chair;
            newMember.ExOfficio = submittedMember.ExOfficio;
            newMember.Year = submittedMember.StartYear;

            if(ModelState.IsValid)
            {
                _context.Add(newMember);
                await _context.SaveChangesAsync();
                Message = "Member Added";
                return RedirectToAction(nameof(Details), new { id = submittedMember.CommitteeId }); 
            }
            var model = await CommitteeMemberAddViewModel.Retry(_context, vm);
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