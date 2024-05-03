namespace DTO.Base
{
    public class BaseGuidDTO : IBaseGuidDTO
    {
        public Guid Id { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
