using Entities.Base;

namespace Entities
{
    public class User : BaseEntity
    {
        public int role_id { get; set; }
        public string? role_name { get; set; }
        public string username { get; set; }
        public string password_salt { get; set; }
        public string password_hash { get; set; }
        public string email { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
        public string? refresh_token { get; set; }
        public DateTimeOffset? token_created { get; set; }
        public DateTimeOffset? token_expires { get; set; }
    }
}
