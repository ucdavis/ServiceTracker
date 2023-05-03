using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceTracker.Helpers;
using ServiceTracker.Models;
using System.Text.Encodings.Web;
using ServiceTracker.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ServiceTracker.Controllers
{
    [Authorize(Roles = "Faculty,Admin")]
    public class CommitmentController : SuperController
    {
        private readonly ServiceTrackerContext _context;

        public CommitmentController(ServiceTrackerContext context)
        {
            _context = context;
        }        

        public async Task<IActionResult> Delete(int id)
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var year = YearFinder.Year + 1;
            var interestToDelete = await _context.CommitteePreferences.Where(p => p.Id == id).FirstOrDefaultAsync();            
            if(interestToDelete == null || interestToDelete.EmployeeId != employeeId || interestToDelete.Year != year)
            {
                ErrorMessage = "Committee not found";
                return RedirectToAction(nameof(Index));
            }
            _context.Remove(interestToDelete);             
            await _context.SaveChangesAsync();
            Message = "Interest deleted";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult New()
        {
            var model = CommitmentViewModel.Create();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> New(EmployeeCommitments commitment)
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var newCommitment  = new EmployeeCommitments();
            newCommitment.EmployeeId = employeeId;
            newCommitment.Title = commitment.Title;
            newCommitment.startYear = commitment.startYear;
            newCommitment.endYear = commitment.endYear;           
            newCommitment.Location = commitment.Location;           
            newCommitment.Description = commitment.Description;
            _context.Add(newCommitment);
            await _context.SaveChangesAsync();
            Message = "Commitment added";
            return RedirectToAction(nameof(InterestController.Index), nameof(InterestController).Replace("Controller",""));

        }

        public async Task<IActionResult> Edit(int id)
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var model = await CommitmentViewModel.Edit(_context, id, employeeId);
            if(model.commitment == null)
            {
                ErrorMessage = "Commitment not found";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CommitmentViewModel editedCommitment)
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var submittedCommitment  = editedCommitment.commitment;
            var commitmentToEdit = await _context.Commitments.Where(c => c.Id == submittedCommitment.Id && c.EmployeeId == employeeId).FirstOrDefaultAsync();
            if(commitmentToEdit == null)
            {
                ErrorMessage = "Commitment not found";
                return RedirectToAction(nameof(Index));
            }
            commitmentToEdit.Title = submittedCommitment.Title;
            commitmentToEdit.startYear = submittedCommitment.startYear;
            commitmentToEdit.endYear = submittedCommitment.endYear;
            commitmentToEdit.Location = submittedCommitment.Location;
            commitmentToEdit.Description = submittedCommitment.Description;
            await _context.SaveChangesAsync();
            Message = "Commitment updated";
            return RedirectToAction(nameof(InterestController.Index), nameof(InterestController).Replace("Controller",""));
        }
    }
}