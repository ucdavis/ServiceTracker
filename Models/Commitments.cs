using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceTracker.Models
{
    public enum LocationsList {
        Department,
        College,
        Campus,
        [Display(Name = "Graduate Group")]
        GraduateGroup,
        [Display(Name ="System Wide")]
        SystemWide,
        External
    }

    public enum DescriptionsList {
        Member,
        Editor,
        [Display(Name ="Company Advisory Board")]
        CompanyAdvisoryBoard,
        Chair,
        CoChair,        
    }
    public class EmployeeCommitments
    {
        public int Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        [Display (Name ="Start Year")]
        [Range(2000,2100)]
        public int startYear { get; set; }
        [Required]
        [Display (Name ="End Year")]
        [Range(2000,2100)]
        public int endYear { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Description { get; set; }
    }
}