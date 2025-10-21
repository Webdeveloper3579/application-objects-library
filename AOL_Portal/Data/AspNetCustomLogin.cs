using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AOL_Portal.Data
{
    [Table("AspNetUserLogins")]
    [PrimaryKey(nameof(LoginProvider), nameof(ProviderKey))]
    public class AspNetCustomLogin
    {
        [Column("LoginProvider")]
        [MaxLength(450)]
        public string LoginProvider { get; set; } = string.Empty;

        [Column("ProviderKey")]
        [MaxLength(450)]
        public string ProviderKey { get; set; } = string.Empty;

        [Column("ProviderDisplayName")]
        [MaxLength(256)]
        public string? ProviderDisplayName { get; set; }

        [Column("UserId")]
        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;
    }
}

