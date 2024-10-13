using esports.Models;

namespace esports.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int Round_Number { get; set; }
        public int Match_In_Round_Number { get; set; }
        public Team? First_Team { get; set; }
        public Team? Second_Team { get; set; }                

        // Foreign key to Tournament
        public int TournamentId { get; set; }
        public required Tournament Tournament { get; set; } 

        public string? WinningTeam { get; set; }

        public bool IsValid(int round_number)
        {


            return (this.First_Team != null)
                && (this.Second_Team != null)
                && (this.Tournament != null)
                && (this.TournamentId >= 0)
                && (this.Round_Number > 0)
                && (this.Match_In_Round_Number > 0);
        }
    }
}