using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    [Table("FieldValues")]
    public class FieldValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(450)] // Standard ASP.NET Identity UserId length
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int ObjectId { get; set; }

        [Required]
        public int FieldId { get; set; }

        [Required]
        [MaxLength(1000)] // Allow for longer values
        public string Value { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual AolApplicationUser? User { get; set; }

        [ForeignKey("ObjectId")]
        public virtual Object? Object { get; set; }

        [ForeignKey("FieldId")]
        public virtual Field? Field { get; set; }
    }
}
