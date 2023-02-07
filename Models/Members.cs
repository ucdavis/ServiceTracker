using System.ComponentModel.DataAnnotations;

namespace ServiceTracker.Models
{
    public class Members
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string Name { 
            get {
                return FirstName + " " + LastName;
            }
        }
    }
}