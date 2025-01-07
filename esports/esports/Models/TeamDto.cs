namespace esports.Models
{
    public class TeamDto
    {
        public string? Name { get; set; }


        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        public TeamDto(string name)
        {
            Name = name;
        }
    }
}
