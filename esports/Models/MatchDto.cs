using System.ComponentModel.DataAnnotations;

namespace esports.Models
{
    public class MatchDto
    {
        [Required(ErrorMessage = "Round number is required")]
        public required int Round_Number { get; set; }
        [Required(ErrorMessage = "Match in round number is required")]
        public required int Match_In_Round_Number { get; set; }
        public int? FirstTeamId { get; set; }
        public Team? FirstTeam { get; set; }
        public int? SecondTeamId { get; set; }
        public Team? SecondTeam { get; set; }
        public int? WinningTeamId { get; set; }
        public Team? WinningTeam { get; set; }
        // Foreign key to Tournament
        public int? TournamentId { get; set; }

        public bool IsValid(int round_count)
        {
            int power = round_count - Round_Number;
            if (Math.Pow(2, power) < Match_In_Round_Number)
            {
                return false;
            }

            return (this.FirstTeamId != null)
                && (this.SecondTeamId != null)                
                && (this.TournamentId >= 0)
                && (this.Round_Number > 0)
                && (this.Match_In_Round_Number > 0);
        }
    }
}
