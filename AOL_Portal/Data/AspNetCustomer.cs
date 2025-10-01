using System.ComponentModel.DataAnnotations;

namespace AOL_Portal.Data
{
    public class AspNetCustomer
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string CustomerType { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        public string CustomerAddress { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
