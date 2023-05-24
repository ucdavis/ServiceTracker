using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceTracker.Helpers;

namespace ServiceTracker.Models
{   
     
    public class InterestIndexViewModel
    {
        public List<CommitteePreference> interests { get; set; }
        public List<EmployeeCommitments> commitments { get; set; }
        public List<EmployeeCommitments> pastCommitments { get; set; }
        public List<Committees> currentAssignments { get; set; }
        public int InterestYear { get; set; }
               
        public static async Task<InterestIndexViewModel> Create(ServiceTrackerContext _context, string employeeId)
        {
            var year = YearFinder.Year +1;
            var model = new InterestIndexViewModel
            {
                interests = await _context.CommitteePreferences.Include(p => p.Committee).Where(p => p.EmployeeId == employeeId && p.Year == year).OrderBy(p => p.PreferenceOrder).ToListAsync(),
                commitments = await _context.Commitments.Where(c => c.EmployeeId == employeeId && c.startYear <= year && c.endYear >= year).ToListAsync(),
                pastCommitments = await _context.Commitments.Where(c => c.EmployeeId == employeeId && c.endYear < year).ToListAsync(),
                currentAssignments = await _context.Committees
                    .Include(c => c.Members.Where(m=> m.EmployeeId == employeeId && m.StartYear <= year && m.EndYear >= year))
                    .Where(c => c.Members.Any(m=> m.EmployeeId == employeeId && m.StartYear <= year && m.EndYear >= year)).ToListAsync(),
                InterestYear = year,
            };           
            
            return model;
        }

       
    }    
}
