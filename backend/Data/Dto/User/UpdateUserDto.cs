namespace HotDeskBookingSystem.Data.Dto.User
{
    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HashPassword { get; set; }
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}
