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
     
    public class CommitmentViewModel
    {
        public EmployeeCommitments commitment { get; set; }
        public List<string> locations { get; set; }      
        public List<string> descriptions { get; set; } 
               
        public static CommitmentViewModel Create()
        {
            var newCommitment = new EmployeeCommitments();
            newCommitment.startYear = YearFinder.Year + 1;
            newCommitment.endYear = YearFinder.Year + 1;               
            var model = new CommitmentViewModel
            {
               commitment = newCommitment,
               locations = EnumHelper.GetListOfDisplayNames<LocationsList>(),
               descriptions = EnumHelper.GetListOfDisplayNames<DescriptionsList>()
            };           
            
            return model;
        }

        public static async Task<CommitmentViewModel> Edit(ServiceTrackerContext _context, int id, string employeeId)
        {
            var model = new CommitmentViewModel
            {
               commitment = await _context.Commitments.Where(c => c.Id == id && c.EmployeeId == employeeId).FirstOrDefaultAsync(),
               locations = EnumHelper.GetListOfDisplayNames<LocationsList>(),
               descriptions = EnumHelper.GetListOfDisplayNames<DescriptionsList>()
            };           
            
            return model;
            
        }

       
    }    
}
