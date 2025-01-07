namespace blazor_frontend.Models
{
    public class RegisterDto {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterDto()
        {
            UserName = "";
            Email = "";
            Password = "";
        }

        public RegisterDto(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
    }
}
