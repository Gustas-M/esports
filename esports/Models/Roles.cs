namespace esports.Models
{
    public class Roles
    {
        public const string Admin = nameof(Admin);
        public const string TeamMember = nameof(TeamMember);
        public const string User = nameof(User);

        public static readonly IReadOnlyCollection<string> AllRoles = new[]
        {
            Admin,
            TeamMember,
            User
        };
    }
}
