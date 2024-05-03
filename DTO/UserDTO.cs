using DTO.Base;

namespace DTO
{
    public class UserDTO : BaseGuidDTO
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset? TokenCreated { get; set; }
        public DateTimeOffset? TokenExpires { get; set; }
    }
}
