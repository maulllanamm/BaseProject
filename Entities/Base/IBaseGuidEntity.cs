namespace Entities.Base
{
    public interface IBaseGuidEntity
    {
        public Guid id { get; set; }
        public bool? is_deleted { get; set; }
        public DateTimeOffset? created_date { get; set; }
        public string created_by { get; set; }
        public DateTimeOffset? modified_date { get; set; }
        public string modified_by { get; set; }
    }
}
