using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    public class AspNetCustomer
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(450)]
        [ForeignKey("AspNetUsers")]
        public string CustomerId { get; set; } = string.Empty; // References AspNetUsers.Id
        
        [Required]
        [ForeignKey("AolCustomerCustomFields")]
        public int CustomFieldId { get; set; } // References AolCustomerCustomFields.CustomerFieldId
        
        [Required]
        [MaxLength(100)]
        public string CustomFieldValue { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        
        // Navigation properties
        public virtual AolApplicationUser AspNetUser { get; set; }
        public virtual AolCustomerCustomField CustomField { get; set; }
    }
}
