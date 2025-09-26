using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    public class AolApplicationUser: IdentityUser
    {
        [Required]
        [PersonalData]
        [Column(TypeName = "varchar(550)")]
        public string FirstName { get; set; }
        [Required]
        [PersonalData]
        [Column(TypeName = "varchar(250)")]
        public string Surname { get; set; }
        [Required]
        public DateTime CreatedDtm { get; set; }
        [Required]
        public DateTime LastUpdateDtm { get; set; }
        [Required]
        public string LastUpdateUserId { get; set; }
        [Required]
        [PersonalData]
        public int StatusId { get; set; }
        public bool? EmailConfirmPasswordChanged { get; set; }
        public bool IsSiteAdmin { get; set; }
    }
}
