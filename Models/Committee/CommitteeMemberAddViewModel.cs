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
     
    public class CommitteMemberAddViewModel
    {
        public Committees  committee { get; set; }

        public CommitteeMembers member { get; set; }
        public List<Employee> faculty { get; set; }

        public List<SelectListItem> years { get; set; }       
        public int ViewYear { get; set; }

        public int AppointmentLength { get; set; }

       
        
               
        public static async Task<CommitteMemberAddViewModel> Create(ServiceTrackerContext _context, int id, int year)
        {
            if(year == 0)
            {
                year = YearFinder.Year;
            }            
            var model = new CommitteMemberAddViewModel
            {
                committee = await _context.Committees.Where(c => c.Id == id).FirstOrDefaultAsync(),
                faculty = await _context.Employees.Where(e => e.VoteCategory != 0).OrderBy(e => e.LastName).ThenBy(e => e.LastName).ToListAsync(),
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
