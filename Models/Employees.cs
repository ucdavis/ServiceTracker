using System.ComponentModel.DataAnnotations;

namespace ServiceTracker.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}