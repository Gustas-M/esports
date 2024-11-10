using esports.Models;
using System.ComponentModel.DataAnnotations;

namespace esports.Models
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

        [Required]
        public required string UserId { get; set; }

        public User User { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(this.Name))
                && (this.Number_of_rounds > 0)
                && (this.Championship != null)
                && (this.ChampionshipId >= 0);
        }
    }
}