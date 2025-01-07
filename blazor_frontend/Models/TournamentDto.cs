namespace blazor_frontend.Models
{
    public class TournamentDto
    {
        public string Name { get; set; }

        public int? Number_of_rounds { get; set; }

        public int ChampionshipId { get; set; }

        public TournamentDto()
        {
            Name = "";
            Number_of_rounds = null;
        }

        public TournamentDto(string name, int number_of_rounds)
        {
            Name = name;
            Number_of_rounds = number_of_rounds;
        }
    }
}
