using esports.Models;

namespace esports.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number_of_rounds { get; set; }

        // Foreign key to Championship
        public int ChampionshipId { get; set; }
        public required Championship Championship { get; set; }        
    }
}