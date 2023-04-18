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
        public int InterestYear { get; set; }
               
        public static async Task<InterestIndexViewModel> Create(ServiceTrackerContext _context, string employeeId)
        {
            var year = YearFinder.Year;
            var model = new InterestIndexViewModel
            {
                interests = await _context.CommitteePreferences.Include(p => p.Committee).Where(p => p.EmployeeId == employeeId && p.Year == year).OrderBy(p => p.PreferenceOrder).ToListAsync(),
                commitments = await _context.Commitments.Where(c => c.EmployeeId == employeeId && c.startYear <= year && c.endYear >= year).ToListAsync(),
                InterestYear = year,
            };           
            
            return model;
        }

       
    }    
}
