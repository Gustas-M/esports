namespace blazor_frontend.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }


        public Team(string? name, string? userId)
        {
            this.Name = name;
            UserId = userId;
        }
    }
}
