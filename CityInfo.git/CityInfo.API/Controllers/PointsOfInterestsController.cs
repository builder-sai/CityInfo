﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

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

        [Route("{cityId}/pointsofinterest/{pointId}", Name = "GetPointOfInterest")]
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
            // adding a custom error when adding the same text in both name and descr property
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided Description should differ from the Name");
            }

            // if one of the properties does not adhere to our PointsofInterestsCreation model we send a bad request
            if (!ModelState.IsValid)
            {
                //returning the ModelState to inform the client what went wrong
                return BadRequest(ModelState);
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

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                pointId = newPointOfIntersst.Id
            }, newPointOfIntersst);
        }

        //When doing a full update use the HttpPut
        [HttpPut("{cityId}/pointsofinterest/{pointId}")]
        public IActionResult UpdatePointOfInterest(int cityId, int pointId,
                   [FromBody] PointsOfInterestsForUpdateDto pointOfInterest)
        {
            //these validations are just for now, fluentValidation should be used for bigger projects
            if (pointOfInterest == null)//could not serialize body
            {
                return BadRequest();
            }
            // adding a custom error when adding the same text in both name and descr property
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided Description should differ from the Name");
            }

            // if one of the properties does not adhere to our PointsofInterestsCreation model we send a bad request
            if (!ModelState.IsValid)
            {
                //returning the ModelState to inform the client what went wrong
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterests.FirstOrDefault(p => p.Id == pointId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            //According to the HTTP Standard a Put should fully update the resource
            //This mean the consumer has to provide all the information needed for the update
            // if the consumer does not provide a value for a field the value should
            //default to it's previous value but we still need to always updates all fields

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            //we could return Ok with the updated content but typically for update you'd return NoContent()
            //noContent says the task is completed successfully but no content to return
            return NoContent();

        }

        [HttpPatch("{cityId}/pointsofinterest/{pointId}")]
        public IActionResult PartiallyUpdatePoint(int cityId, int pointId,
            //from micrsofit.aspnetcore.jsonpatch to support the jsonpatch operations
            [FromBody] JsonPatchDocument<PointsOfInterestsForUpdateDto> patchDoc)
        {
            if (patchDoc == null)//could not serialize body
            {
                return BadRequest();
            }

            // if one of the properties does not adhere to our PointsofInterestsCreation model we send a bad request
            if (!ModelState.IsValid)
            {
                //returning the ModelState to inform the client what went wrong
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterests.FirstOrDefault(p => p.Id == pointId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointsOfInterestsForUpdateDto
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            //adding the ModelState to the ApplyTo to add any error to the ModelState's error collection
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            //We try to ApplyTo and if there 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{pointId}")]
        public IActionResult DeletePointOfInterest(int cityId, int pointId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound(nameof(city));
            }

            var pointToBeDeleted = city.PointOfInterests.FirstOrDefault(p => p.Id == pointId);

            if (pointToBeDeleted == null)
            {
                return NotFound(nameof(pointToBeDeleted));
            }

            city.PointOfInterests.Remove(pointToBeDeleted);

            return NoContent();
        }
    }
}