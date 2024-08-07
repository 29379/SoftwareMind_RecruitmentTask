namespace HotDeskBookingSystem.Data.Dto
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HashPassword { get; set; }
        public List<string> Roles { get; set; }
    }
}
