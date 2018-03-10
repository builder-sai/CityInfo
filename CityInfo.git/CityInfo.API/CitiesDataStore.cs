using System;
using System.Collections.Generic;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        //this line of code make sure that we keep working
        //on the same data as long as we don't restart
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities
        {
            get;
            set;
        }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>(){
                new CityDto(){
                    Id = 1,
                    Name = "Stockholm",
                    Description = "is in Sweden",
                    PointOfInterests = new List<PointOfInterestsDto>()
                    {
                        new PointOfInterestsDto {
                        Id = 1,
                        Name = "sveavägen",
                        Description= "long street in sthlm"
                    },
                        new PointOfInterestsDto {
                        Id = 2,
                        Name = "vintergatan?",
                        Description= "a street in sthlm"
                        },
                    }
                },
                new CityDto(){
                    Id = 2,
                    Name = "NY",
                    Description = "is the USA",
                    PointOfInterests = new List<PointOfInterestsDto>(){
                        new PointOfInterestsDto{
                            Id = 1,
                            Name = "central park",
                            Description= "a park"
                        },
                        new PointOfInterestsDto{
                            Id = 2,
                            Name = "da hood",
                            Description= "not a park..."
                        }
                    }
                },
                new CityDto(){
                    Id = 3,
                    Name = "Paris",
                    Description = "is in France",
                    PointOfInterests = new List<PointOfInterestsDto>(){
                        new PointOfInterestsDto{
                            Id = 1,
                            Name = "Eiffel tower",
                            Description =" an iron tower"
                        },
                        new PointOfInterestsDto{
                            Id = 2,
                            Name = "Not Eiffel tower",
                            Description ="Not an iron tower"
                        }
                    }
                }
            };
        }
    }
}
