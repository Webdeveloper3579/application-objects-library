using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Portal.Data
{
    [Table("City")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CityId")]
        public int CityId { get; set; }
        
        [Required]
        [MaxLength(100)]
        [Column("CityName")]
        public string CityName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        [Column("CityCode")]
        public string CityCode { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        [Column("CityCounty")]
        public string CityCounty { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        [Column("CityCountry")]
        public string CityCountry { get; set; } = string.Empty;
        
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        [Column("ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }
}

