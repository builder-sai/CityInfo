using System;
using System.Collections.Generic;
namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPointsOfInterest
        {

            get { return PointOfInterests.Count; }
        }
        public ICollection<PointOfInterestsDto> PointOfInterests
        {
            get;
            set;
        } = new List<PointOfInterestsDto>();
        //using c# 6 auto-property initilaizer syntax the point of interest to an 
        //empty list instead of leaving it at null.


    }
}
