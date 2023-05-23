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
     
    public class AssignmentViewModel
    {
        public List<Committees>  committees { get; set; }
        public List<CommitteePreference> interest { get; set; }               
        public int ViewYear { get; set; }

       
        
               
        public static async Task<AssignmentViewModel> Create(ServiceTrackerContext _context)
        {
            var year = YearFinder.Year + 1;                     
            var model = new AssignmentViewModel
            {
                committees = await _context.Committees.Include(c => c.Members.Where(m => m.StartYear <= year && m.EndYear >= year)).ThenInclude(m => m.Employee).Include(c => c.Members.Where(m => m.StartYear <= year && m.EndYear >= year)).ThenInclude(m => m.Member).ToListAsync(),                
                interest = await _context.CommitteePreferences.Include(p => p.Employee).Where(p => p.Year == year).ToListAsync(),
                ViewYear = year,                              
            };           
            
            return model;
        }

       
    }    
}