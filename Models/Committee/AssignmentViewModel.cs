using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ServiceTracker.Helpers;

namespace ServiceTracker.Models
{   
     
    public class AssignmentViewModel
    {
        public List<Committees>  committees { get; set; }
        public List<CommitteePreference> interest { get; set; }               
        public int ViewYear { get; set; }
        public List<Employee> slackers { get; set; }
        public List<Employee> noResponders { get; set; }




        public static async Task<AssignmentViewModel> Create(ServiceTrackerContext _context)
        {
            var year = YearFinder.Year + 1;    
            var currentMembers = await _context.CommitteeMembers.Where(m => m.StartYear <= year && m.EndYear >= year).Select(m=> m.EmployeeId).ToListAsync();
            var listExclusion = new List<int>(new int[] {11,12,13,14,21});
            var currentResponders = await _context.CommitteePreferences.Where(p => p.Year == year).Select(m=> m.EmployeeId).ToListAsync();
            var model = new AssignmentViewModel
            {
                committees = await _context.Committees.Include(c => c.Members.Where(m => m.StartYear <= year && m.EndYear >= year)).ThenInclude(m => m.Employee).Include(c => c.Members.Where(m => m.StartYear <= year && m.EndYear >= year)).ThenInclude(m => m.Member).ToListAsync(),                
                interest = await _context.CommitteePreferences.Include(p => p.Employee).Where(p => p.Year == year).ToListAsync(),
                ViewYear = year,                   
                slackers = await _context.Employees.Where(e => e.VoteCategory != 0 && !currentMembers.Contains(e.Id) && !listExclusion.Contains(e.VoteCategory) && e.Current).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToListAsync(),          
                noResponders = await _context.Employees.Where(e => e.VoteCategory != 0 && !currentResponders.Contains(e.Id) && !listExclusion.Contains(e.VoteCategory) && e.Current).OrderBy(e=>e.LastName).ThenBy(e=>e.FirstName).ToListAsync(),
            };           
            
            return model;
        } 

        public static async Task<AssignmentViewModel> CreateReport(ServiceTrackerContext _context)
        {
            var year = YearFinder.Year + 1;
            var currentMembers = await _context.CommitteeMembers.Where(m => m.StartYear <= year && m.EndYear >= year).Select(m => m.EmployeeId).ToListAsync();
            var listExclusion = new List<int>(new int[] { 11, 12, 13, 14, 21 });
            var currentResponders = await _context.CommitteePreferences.Where(p => p.Year == year).Select(m => m.EmployeeId).ToListAsync();
            var model = new AssignmentViewModel
            {
                committees = await _context.Committees.Include(c => c.Members.Where(m => m.StartYear <= year && m.EndYear >= year)).ThenInclude(m => m.Employee).Include(c => c.Members.Where(m => m.StartYear <= year && m.EndYear >= year)).ThenInclude(m => m.Member).ToListAsync(),                
                ViewYear = year,
                slackers = await _context.Employees.Where(e => e.VoteCategory != 0 && !listExclusion.Contains(e.VoteCategory) && e.Current).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToListAsync(),
                
            };

            return model;
        }

       
    }    
}