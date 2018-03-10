using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.API.Controllers
{
    //points of interests are not known by it's own 
    //that's why we need to access the city to get it's points of interests
    // (api/citis/cityId/pointsofinterests
    [Route("api/cities")]
    public class PointsOfInterestsController : Controller
    {
        [HttpGet("{cityId}/pointsofinterests")]
        public IActionResult GetPointsOfInterests(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.PointOfInterests);
        }

        [Route("{cityId}/pointsofinterests/{pointId}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int pointId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var points = city.PointOfInterests.FirstOrDefault(p => p.Id == pointId);
            if (points == null)
            {
                return NotFound();
            }
            return Ok(points);
        }
        [HttpPost("{cityId}/pointsofinterests")]
        public IActionResult CreatePointOfInterest(int cityId,
                       [FromBody] PointsOfInterestsForCreationDto pointOfInterest)
        {
            //a problem that might occur is that the post request might not be deserialized into the point of interests for creation dto
            // if it cant be deserialized then the point of interests parameter will be null
            // for that we return a bad request because it's the consumer's error
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            // if one of the properties does not adhere to our PointsofInterestsCreation model we send a bad request
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //the consumer might send a request to a resource URI that does not exists
            //e.g. adding a point of interests to a non-existing city so we check for that
            //and return not found if the city does not exists
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            //new we can create the new point of interest
            // *for now* we need to calculate the id for the new-to-be-created point of interest
            // calculate the id by running through all the current point
            //of interests accross cities and getting the heighest value
            var maxPointOfInterest = CitiesDataStore.Current.Cities.SelectMany(p => p.PointOfInterests).Max(i => i.Id);

            var newPointOfIntersst = new PointOfInterestsDto()
            {
                //we add one (1) and we use it when creating a new point of interest
                Id = ++maxPointOfInterest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointOfInterests.Add(newPointOfIntersst);

            return CreatedAtRoute("GetPointOfInterest", new{
                cityId = cityId, pointId = newPointOfIntersst.Id
            }, newPointOfIntersst);
        }
    }
}
