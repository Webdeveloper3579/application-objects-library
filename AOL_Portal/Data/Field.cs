using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    [Table("Field")]
    public class Field
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("ObjectId")]
        public int ObjectId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("FieldLabel")]
        public string FieldLabel { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("FieldType")]
        public string FieldType { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("FieldDescription")]
        public string FieldDescription { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("FieldName")]
        public string FieldName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("FieldStatus")]
        public string FieldStatus { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("FieldEnum")]
        public string FieldEnum { get; set; } = string.Empty;

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation property
        [ForeignKey("ObjectId")]
        public virtual Object? Object { get; set; }
    }
}
