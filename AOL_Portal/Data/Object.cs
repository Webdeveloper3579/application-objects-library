using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    [Table("Object")]
    public class Object
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("ObjectName")]
        public string ObjectName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("ObjectEnum")]
        public string ObjectEnum { get; set; } = string.Empty;

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }
}
