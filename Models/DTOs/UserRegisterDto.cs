namespace AWING.TreasureHuntAPI.Models.DTOs
{
    public class UserRegisterDto
    {
        private string userName;
        public string UserName
        {
            get { return userName.Trim(); }
            set { userName = value.Trim(); }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Password { get; set; }
    }
    public class UserLoginDto
    {
        private string userName;

        public string UserName
        {
            get { return userName.Trim(); }
            set { userName = value.Trim(); }
        }

        public string Password { get; set; }
    }
}
