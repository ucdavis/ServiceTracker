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
     
    public class CommitteeDetailsViewModel
    {
        public Committees  committee { get; set; }

        public List<SelectListItem> years { get; set; }       
        public int ViewYear { get; set; }

       
        
               
        public static async Task<CommitteeDetailsViewModel> Create(ServiceTrackerContext _context, int id, int year)
        {
            if(year == 0)
            {
                year = YearFinder.Year;
            }            
            var model = new CommitteeDetailsViewModel
            {
                committee = await _context.Committees
                    .Include(c => c.Members.Where(m=> m.StartYear <= year && m.EndYear >= year)).ThenInclude(m => m.Employee)
                    .Include(c => c.Members.Where(m=> m.StartYear <= year && m.EndYear >= year)).ThenInclude(m => m.Member)
                    .Where(c => c.Id == id).FirstOrDefaultAsync(),
                ViewYear = year,              
                years = YearFinder.YearList.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.ToString(),
                            Value = a.ToString()
                        };
                    })
            };           
            
            return model;
        }

       
    }    
}
