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
     
    public class CommitteeMemberAddViewModel
    {
        public Committees  committee { get; set; }

        public CommitteeMembers member { get; set; }
        public List<Employee> faculty { get; set; }

        public List<SelectListItem> years { get; set; }       
       

        public int AppointmentLength { get; set; }

       
        
               
        public static async Task<CommitteeMemberAddViewModel> Create(ServiceTrackerContext _context, int id, int year)
        {
            if(year == 0)
            {
                year = YearFinder.Year;
            }            
            var model = new CommitteeMemberAddViewModel
            {
                committee = await _context.Committees.Where(c => c.Id == id).FirstOrDefaultAsync(),
                faculty = await _context.Employees.Where(e => e.VoteCategory != 0).OrderBy(e => e.LastName).ThenBy(e => e.LastName).ToListAsync(),
                 
                member = new CommitteeMembers{CommitteeId = id, StartYear = year},             
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

        public static async Task<CommitteeMemberAddViewModel> Retry(ServiceTrackerContext _context, CommitteeMemberAddViewModel vm)
        {              
            vm.committee =   await _context.Committees.Where(c => c.Id == vm.member.CommitteeId).FirstOrDefaultAsync();
            vm.faculty = await _context.Employees.Where(e => e.VoteCategory != 0).OrderBy(e => e.LastName).ThenBy(e => e.LastName).ToListAsync();
            vm.years = YearFinder.YearList.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.ToString(),
                            Value = a.ToString()
                        };
                    });
            
            return vm;
        }

       
    }    
}
