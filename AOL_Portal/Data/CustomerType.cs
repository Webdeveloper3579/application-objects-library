using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    [Table("CustomerType")]
    public class CustomerType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CustomerTypeId")]
        public int CustomerTypeId { get; set; }
        
        [Required]
        [MaxLength(100)]
        [Column("CustomerTypeName")]
        public string CustomerTypeName { get; set; } = string.Empty;
    }
}
