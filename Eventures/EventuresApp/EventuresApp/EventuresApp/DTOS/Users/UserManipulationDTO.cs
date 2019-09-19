namespace EventuresApp.DTOS.Users
{
    public class UserManipulationDTO
    {
        public string Id { get; set; }
        public Operation Operation { get; set; }
    }
    public enum Operation
    {
        Promote=1,Demote=2
    }
}