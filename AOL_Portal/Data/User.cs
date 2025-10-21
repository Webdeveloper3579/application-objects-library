using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UserId")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("LastName")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        [Column("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("IsSiteAdmin")]
        public bool IsSiteAdmin { get; set; } = false;
    }
}

