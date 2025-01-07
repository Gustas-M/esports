
namespace blazor_frontend.Models
{
    public class ChampionshipDto
    {
        public string Name { get; set; }        
        public int? Year { get; set; }

        public ChampionshipDto()
        {
            Name = "";
            Year = null;
        }

        public ChampionshipDto(string name, int year)
        {
            Name = name;
            Year = year;
        }
    }
}
