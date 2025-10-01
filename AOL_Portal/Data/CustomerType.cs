using System.ComponentModel.DataAnnotations;

namespace AOL_Portal.Data
{
    public class CustomerType
    {
        [Key]
        public int CustomerTypeId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CustomerTypeName { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
