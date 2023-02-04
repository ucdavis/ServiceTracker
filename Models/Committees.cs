using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceTracker.Models
{
    public class Committees 
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1,10)]
        public int Term { get; set; }


        [ForeignKey("CommitteeId")]
        public ICollection<CommitteeMembers> Members { get; set; }
    }
}