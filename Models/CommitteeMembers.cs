using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceTracker.Models
{
    public class CommitteeMembers
    {
        public int Id { get; set; }
        public int CommitteeId { get; set; }
        [Required]
        public string? EmployeeId { get; set; }
        public bool Chair { get; set; }
        public bool ExOfficio { get; set; }
        public int Year { get; set; }

        
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        
    }
}