using esports.Models;
using System.ComponentModel.DataAnnotations;

namespace esports.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int Round_Number { get; set; }
        public int Match_In_Round_Number { get; set; }
        public int? FirstTeamId { get; set; }
        public int? SecondTeamId { get; set; }
        public int? WinningTeamId { get; set; }
        // Foreign key to Tournament
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }



        public bool IsValid(int round_count)
        {
            int power = round_count - Round_Number + 1;
            if (Math.Pow(2, power) < Match_In_Round_Number)
            {
                return false;
            }

            return (this.FirstTeamId != null)
                && (this.SecondTeamId != null)
                && (this.Tournament != null)
                && (this.TournamentId >= 0)
                && (this.Round_Number > 0)
                && (this.Match_In_Round_Number > 0);
        }
    }
}