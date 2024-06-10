using Entities.Base;

namespace Entities
{
    public class Dummy : BaseGuidEntity
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string full_name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
        public Guid cart_id { get; set; }
        public string gender { get; set; }

    }

}
