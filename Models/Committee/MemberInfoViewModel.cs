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
     
    public class MemberInfoViewModel
    {
        public List<EmployeeCommitments>  commitments { get; set; }
        public List<CommitteePreference> interest { get; set; }
        public List<Committees>   assignments { get; set; }

       
        
               
        public static async Task<MemberInfoViewModel> Create(ServiceTrackerContext _context, string id)
        {
            var year = YearFinder.Year + 1;                     
            var model = new MemberInfoViewModel
            {
                commitments = await _context.Commitments.Where(c => c.EmployeeId == id && c.startYear <= year && c.endYear >= year).ToListAsync(),                
                interest = await _context.CommitteePreferences.Include(p => p.Committee).Where(p => p.Year == year && p.EmployeeId == id).OrderBy(p => p.PreferenceOrder).ToListAsync(),
                assignments = await _context.Committees.AsNoTracking().Include(c => c.Members.Where(m => (m.EmployeeId == id || m.MemberId == int.Parse(id)) && m.StartYear <= year && m.EndYear >= year)).Where(c => c.Members.Any(m => m.EmployeeId == id || m.MemberId == int.Parse(id))).ToListAsync()
            };           
            
            return model;
        }

       
    }    
}