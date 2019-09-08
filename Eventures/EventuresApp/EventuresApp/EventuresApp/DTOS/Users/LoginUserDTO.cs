using System.ComponentModel.DataAnnotations;

namespace EventuresApp.DTOS.Users
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "You must give username!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "You must give password!")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}