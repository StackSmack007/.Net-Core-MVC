namespace Panda_Exam.DTOS.Users
{
    public class RegisterUserDto:LoginUserDTO
    {
        public string VerifyPassword { get; set; }
        public string Email { get; set; }
    }
}