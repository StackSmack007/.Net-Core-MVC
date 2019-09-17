namespace EventuresApp.DTOS.Users
{
    using System.ComponentModel.DataAnnotations;
    public class RegisterUserDto : LoginUserDto
    {
        [Required]
        [MinLength(3)]
        [RegularExpression(@"[^\d\-\*]+", ErrorMessage = "Symbols not allowed")]
        public string UserName { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"\d{10}", ErrorMessage = "Should contain 10 digits only")]
        public string UCN { get; set; }

    }
}