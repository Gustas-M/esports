using esports.Models;

namespace esports.Models
{
    public class Match
    {
        public int Id { get; set; }

        // Foreign key to Tournament
        public int TournamentId { get; set; }
        public required Tournament Tournament { get; set; }

        public string? WinningTeam { get; set; }
    }
}