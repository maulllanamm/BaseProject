using DTO.Base;

namespace DTO
{
    public class PermissionDTO : BaseDTO
    {
        public string Name { get; set; }
        public string HttpMethod { get; set; }
        public string Path { get; set; }
    }
}
