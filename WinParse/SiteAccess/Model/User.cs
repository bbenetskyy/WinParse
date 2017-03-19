namespace SiteAccess.Model
{
    public class User
    {
        public string Login { get; set; }// = "yuravashchenko15@gmail.com";
        public string Password { get; set; }// = @"Ad2_w3Ddsg";
        public bool Logged { get; set; }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}