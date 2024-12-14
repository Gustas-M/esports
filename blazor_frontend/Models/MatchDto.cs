using System.ComponentModel.DataAnnotations;

namespace blazor_frontend.Models
{
    public class MatchDto
    {       
        public int? Round_Number { get; set; }        
        public int? Match_In_Round_Number { get; set; }
        public int? FirstTeamId { get; set; }
        public Team? FirstTeam { get; set; }
        public int? SecondTeamId { get; set; }
        public Team? SecondTeam { get; set; }
        public int? WinningTeamId { get; set; }
        public Team? WinningTeam { get; set; }
        // Foreign key to Tournament
        public int? TournamentId { get; set; }


        public MatchDto()
        {
            Round_Number = null;
            Match_In_Round_Number = null;
            FirstTeamId = null;
            FirstTeam = null;
            SecondTeamId = null;
            SecondTeam = null;
            WinningTeamId = null;
            WinningTeam = null;
            TournamentId = null;
        }

        public MatchDto(int round_Number, int match_In_Round_Number, int? firstTeamId, Team? firstTeam, int? secondTeamId, Team? secondTeam, int? winningTeamId, Team? winningTeam, int? tournamentId)
        {
            Round_Number = round_Number;
            Match_In_Round_Number = match_In_Round_Number;
            FirstTeamId = firstTeamId;
            FirstTeam = firstTeam;
            SecondTeamId = secondTeamId;
            SecondTeam = secondTeam;
            WinningTeamId = winningTeamId;
            WinningTeam = winningTeam;
            TournamentId = tournamentId;
        }
    }
}
