namespace blazor_frontend.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int Round_Number { get; set; }
        public int Match_In_Round_Number { get; set; }
        public int? FirstTeamId { get; set; }
        public Team? FirstTeam { get; set; }
        public int? SecondTeamId { get; set; }
        public Team? SecondTeam { get; set; }
        public int? WinningTeamId { get; set; }
        public Team? WinningTeam { get; set; }
        // Foreign key to Tournament
        public int? TournamentId { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
