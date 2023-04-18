using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceTracker.Models
{
    public class EmployeeCommitments
    {
        public int Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public int startYear { get; set; }
        [Required]
        public int endYear { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string location { get; set; }
        [Required]
        public string description { get; set; }
    }
}