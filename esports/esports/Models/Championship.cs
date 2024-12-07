using esports.Models;
using System.ComponentModel.DataAnnotations;

namespace esports.Models
{
    public class Championship
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Year { get; set; }


        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Name)) && (Year > 0);
        }
    }
}