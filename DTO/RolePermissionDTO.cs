using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class RolePermissionDTO
    {
        [Key]
        public int RoleId { get; set; }
        [Key]
        public int PermissionId { get; set; }
    }
}
