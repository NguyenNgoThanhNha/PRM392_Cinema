namespace PRM_API.Dtos
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? Fullname { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
