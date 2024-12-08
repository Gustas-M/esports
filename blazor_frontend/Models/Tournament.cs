namespace blazor_frontend.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int Number_of_rounds { get; set; }

        // Foreign key to Championship
        public int ChampionshipId { get; set; }

        // Navigation property to Championship
        public Championship Championship { get; set; }
    }
}
