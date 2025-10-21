using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    [Table("SelectValues")]
    public class SelectValue
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("FieldId")]
        public int FieldId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("Value")]
        public string Value { get; set; } = string.Empty;

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation property
        [ForeignKey("FieldId")]
        public virtual Field? Field { get; set; }
    }
}
