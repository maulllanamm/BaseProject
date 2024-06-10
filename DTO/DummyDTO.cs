using DTO.Base;

namespace DTO
{
    public class DummyDTO : BaseGuidDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public Guid CartId { get; set; }
        public string Gender { get; set; } 
    }


    public enum GenderType
    {
        Male,
        Female,
    }

}
