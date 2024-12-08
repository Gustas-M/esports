using System.ComponentModel.DataAnnotations;

namespace esports.Models
{
    public class TournamentDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Number of rounds is required")]
        public int? Number_of_rounds { get; set; }

        // Foreign key to Championship
        [Required(ErrorMessage = "Championship is required")]
        public int? ChampionshipId { get; set; } 

        // Optional: Include Championship details if needed
        // public ChampionshipDto Championship { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(this.Name))
                && ((this.Number_of_rounds > 0) || (this.Number_of_rounds != null))
                && (this.ChampionshipId != null);
        }
    }
}
