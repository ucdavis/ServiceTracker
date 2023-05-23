using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceTracker.Models
{
    public class CommitteeMembers
    {
        public int Id { get; set; }
        public int CommitteeId { get; set; }
       
        [StringLength(10)]
        public string EmployeeId { get; set; }
        public int? MemberId { get; set; }
        public bool Chair { get; set; }
        public bool ExOfficio { get; set; }
        public int Year { get; set; }
        public int EndYear { get; set; }
        public int StartYear { get; set; }

        public string ListName { 
            get {
                if(Employee != null)
                {
                    return Employee.FirstName + " " + Employee.LastName;
                }
                if(Member != null)
                {
                    return Member.FirstName + " " + Member.LastName;
                }
                return "Unknown name";
            }
        }

        public string SortName { 
            get {
                if(Employee != null)
                {
                    return Employee.LastName + " " + Employee.FirstName;
                }
                if(Member != null)
                {
                    return Member.LastName + " " + Member.FirstName;
                }
                return "ZZZZ Unknown name";
            }
        }

        public string PersonId { 
            get {
                if(Employee != null)
                {
                    return Employee.Id;
                }
                if(Member != null)
                {
                    return Member.Id.ToString();
                }
                return "ZZZZ Unknown name";
            }
        }



        
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        [ForeignKey("MemberId")]
        public Members Member { get; set; }
        
        
    }
}