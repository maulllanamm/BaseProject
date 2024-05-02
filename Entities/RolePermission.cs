using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class RolePermission
    {
        [Key]
        public int role_id { get; set; }
        [Key]
        public int permission_id { get; set; }
    }
}
