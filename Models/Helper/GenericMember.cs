using System.ComponentModel.DataAnnotations;

namespace ServiceTracker.Models
{
    public class GenericMember
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       

        public string Name { 
            get {
                return FirstName + " " + LastName;
            }
        }

        public string LastFirstName { 
            get {
                return LastName + ", " + FirstName;
            }
        }
    }
}