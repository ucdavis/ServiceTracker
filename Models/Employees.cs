using System.ComponentModel.DataAnnotations;

namespace ServiceTracker.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Int16 VoteCategory { get; set; }
        public bool AdminStaff { get; set; }
        public bool Current { get; set; }
        public string KerberosId { get; set; }
        public bool Chair { get; set; }

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