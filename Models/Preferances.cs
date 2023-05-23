using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceTracker.Models
{
    public class CommitteePreference
    {
        public int Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int CommitteeId { get; set; }
        [Required]
        public int PreferenceOrder { get; set; }

        [ForeignKey("CommitteeId")]
        public Committees Committee { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}