namespace DataSaver.Models
{
    public class User
    {
        public string Id { get; set; }
        public string LoginPinnacle { get; set; }
        public string PasswordPinnacle { get; set; }
        public string LoginMarathon { get; set; }
        public string PasswordMarathon { get; set; }

        public string AntiGateCode { get; set; }
    }
}
