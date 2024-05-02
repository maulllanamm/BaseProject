namespace DTO.Base
{
    public class BaseDTO : IBaseDTO
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
