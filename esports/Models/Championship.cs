using esports.Models;

namespace esports.Models
{
    public class Championship
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Year { get; set; }

        public ICollection<Tournament>? Tournaments { get; set; }
    }
}