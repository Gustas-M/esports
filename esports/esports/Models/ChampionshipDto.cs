using System.ComponentModel.DataAnnotations;

namespace esports.Models
{
    public class ChampionshipDto
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Year is required")]
        public required int? Year { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrWhiteSpace(Name)) && ((Year > 0) || (Year != null));
        }
    }
}
