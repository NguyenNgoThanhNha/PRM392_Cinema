namespace PRM_API.Dtos
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Fullname { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
