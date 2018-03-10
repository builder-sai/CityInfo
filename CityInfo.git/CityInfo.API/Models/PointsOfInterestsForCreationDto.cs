using System;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointsOfInterestsForCreationDto
    {
        [Required(ErrorMessage = "The name property is required")]
        [MaxLength(20)]
        public string Name
        {
            get;
            set;
        }
        [MaxLength(50)]
        public string Description
        {
            get;
            set;
        }

    }
}
