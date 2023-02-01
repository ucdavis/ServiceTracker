using System.ComponentModel.DataAnnotations;

namespace ServiceTracker.Models
{
    public class Committees 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Term { get; set; }
    }
}