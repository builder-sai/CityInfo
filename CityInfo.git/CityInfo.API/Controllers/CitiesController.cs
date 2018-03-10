using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    //[Route("api/[controller]")] <-- this is a website approach, for api:s use "hardcoded links"
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        //changing from JsonResult to an IActionResult to better handle the code status
        // i.g. (200 code stats = ok, 404 not found and such)
        //using IActionResult help us send our data in other formats that the API
        // supports instead of only json in the JsonResult
        [HttpGet()]
        public IActionResult getCities()
        {
            //there is no not found here because even an empty list
            // is a valid response body
            return Ok(CitiesDataStore.Current.Cities);
        }
        [HttpGet("{id}")]
        public IActionResult getCity(int id){
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id);
            if (cityToReturn == null)
            {
                return NotFound();
            }
            return Ok(cityToReturn);
        }
    }
}
