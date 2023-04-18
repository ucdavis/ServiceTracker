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
    public class InterestController : SuperController
    {
        private readonly ServiceTrackerContext _context;

        public InterestController(ServiceTrackerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var model = await InterestIndexViewModel.Create(_context, employeeId);
            return View(model);
        }
        
        public async Task<IActionResult> MoveUp(int id)
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var year = YearFinder.Year;
            var interestToMove = await _context.CommitteePreferences.Where(p => p.Id == id).FirstOrDefaultAsync();
            var interestBumped = await _context.CommitteePreferences.Where(p => p.EmployeeId == employeeId && p.Year == year && p.PreferenceOrder == (interestToMove.PreferenceOrder -1)).FirstOrDefaultAsync();
            if(interestToMove.PreferenceOrder == 1 || interestToMove == null || interestBumped == null || interestToMove.EmployeeId != employeeId || interestToMove.Year != year)
            {
                ErrorMessage = "Committee not found or already at top position";
                return RedirectToAction(nameof(Index));
            }
            interestToMove.PreferenceOrder = interestToMove.PreferenceOrder -1;
            interestBumped.PreferenceOrder = interestBumped.PreferenceOrder + 1;
            await _context.SaveChangesAsync();

            Message = "Committee order updated";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MoveDown(int id)
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var year = YearFinder.Year;
            var interestToMove = await _context.CommitteePreferences.Where(p => p.Id == id).FirstOrDefaultAsync();
            var interestBumped = await _context.CommitteePreferences.Where(p => p.EmployeeId == employeeId && p.Year == year && p.PreferenceOrder == (interestToMove.PreferenceOrder + 1)).FirstOrDefaultAsync();
            var maxInterest = await _context.CommitteePreferences.Where(p => p.EmployeeId == employeeId && p.Year == year).MaxAsync(p => p.PreferenceOrder);
            if(interestToMove.PreferenceOrder == maxInterest || interestToMove == null || interestBumped == null || interestToMove.EmployeeId != employeeId || interestToMove.Year != year)
            {
                ErrorMessage = "Committee not found or already at last position";
                return RedirectToAction(nameof(Index));
            }
            interestToMove.PreferenceOrder = interestToMove.PreferenceOrder +1;
            interestBumped.PreferenceOrder = interestBumped.PreferenceOrder - 1;
            await _context.SaveChangesAsync();

            Message = "Committee order updated";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var year = YearFinder.Year;
            var interestToDelete = await _context.CommitteePreferences.Where(p => p.Id == id).FirstOrDefaultAsync();
            var interestsToReorder = await _context.CommitteePreferences.Where(p => p.EmployeeId == employeeId && p.Year == year && p.PreferenceOrder >= (interestToDelete.PreferenceOrder + 1)).ToListAsync();
            if(interestToDelete == null || interestToDelete.EmployeeId != employeeId || interestToDelete.Year != year)
            {
                ErrorMessage = "Committee not found";
                return RedirectToAction(nameof(Index));
            }
            _context.Remove(interestToDelete);
            if(interestsToReorder != null)
            {
                interestsToReorder.ForEach(p => p.PreferenceOrder = p.PreferenceOrder -1);
            }            
            await _context.SaveChangesAsync();
            Message = "Interest deleted";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> New()
        {
            var model = await _context.Committees.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> New(int id)
        {
            var employeeId = User.FindFirst(ClaimTypes.Sid).Value;
            var year = YearFinder.Year;
            var maxExistingInterest = await _context.CommitteePreferences.Where(p => p.EmployeeId == employeeId && p.Year == year).MaxAsync(p => (int?)p.PreferenceOrder);
            var newInterest = new CommitteePreference();
            newInterest.CommitteeId = id;
            newInterest.EmployeeId = employeeId;
            newInterest.Year = year;
            if(maxExistingInterest == null)
            {
                newInterest.PreferenceOrder = 1;
            } else 
            {
                newInterest.PreferenceOrder = maxExistingInterest.Value + 1;
            }
            _context.Add(newInterest);
            await _context.SaveChangesAsync();
            Message = "Interest added";
            return RedirectToAction(nameof(Index));

        }
    }
}