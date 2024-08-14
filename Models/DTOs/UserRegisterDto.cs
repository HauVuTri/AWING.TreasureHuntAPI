using System.ComponentModel.DataAnnotations;

namespace AWING.TreasureHuntAPI.Models.DTOs
{
    public class UserRegisterDto
    {
        private string userName;
        private string email;

        [Required]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
        public string UserName
        {
            get { return userName; }
            set { userName = value.Trim(); }
        }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email
        {
            get { return email ; }
            set { email = value.Trim() ; }
        }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
    }
    public class UserLoginDto
    {
        private string userName;
        [Required]
        public string UserName
        {
            get { return userName.Trim(); }
            set { userName = value.Trim(); }
        }
        [Required]
        public string Password { get; set; }
    }
}
