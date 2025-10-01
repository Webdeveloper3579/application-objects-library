using System.ComponentModel.DataAnnotations;

namespace AOL_Portal.Data
{
    public class AolCustomerCustomField
    {
        [Key]
        public int CustomerFieldId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CustomerCustomFieldName { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerCustomType { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerCustomFieldLabel { get; set; }

        [Required]
        [MaxLength(500)]
        public string CustomerCustomFieldDescription { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CustomerCustomFieldType { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerCustomFieldStatus { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
