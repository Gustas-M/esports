namespace blazor_frontend.Models
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginDto()
        {
            UserName = "";
            Password = "";
        }

        public LoginDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
