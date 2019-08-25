namespace ChushkaApp.DTOS.Users
{
    public class inputUserRegisterDTO : inputUserLoginDTO
    {
        public string VerifyPassword { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}