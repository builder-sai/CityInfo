using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //City as the navigation property from PointOfInterest to it's
        //parent property City
        [ForeignKey("CityId")]
        public City City { get; set; }

        public int CityId { get; set; }

        //now added a nuget package (Microsoft.EntityFrameworkCore.SqlServer
    }
}
