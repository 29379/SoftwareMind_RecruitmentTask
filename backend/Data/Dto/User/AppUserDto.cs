namespace HotDeskBookingSystem.Data.Dto.User
{
    public class AppUserDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
    }
}
