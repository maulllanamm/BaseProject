using Entities.Base;

namespace Entities
{
    public class Permission : BaseEntity
    {
        public string name { get; set; }
        public string http_method { get; set; }
        public string path { get; set; }
    }
}
