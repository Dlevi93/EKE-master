using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EKE.Service.Services.Vt;
using EKE_WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace EKE_WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IVtServices _vtServices;

        public ValuesController(IVtServices vtServices)
        {
            _vtServices = vtServices;
        }

        [HttpGet("[action]")]
        public IEnumerable<UserResponse> UserList()
        {
            var result = _vtServices.GetAllUsers();
            return result.Data.Select(x => new UserResponse
            {
                Name = x.Name,
                City = x.City,
                Member = x.Membership?.Name.ToString() ?? "-",
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<MembershipResponse> Memberships()
        {
            var result = _vtServices.GetAllMemberships();
            return result.Data.Select(x => new MembershipResponse
            {
                Id = x.Id,
                Enum = (int)x.Membership,
                Name = x.Name
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<AccomodationTypeResponse> AccomodationTypes()
        {
            var result = _vtServices.GetAllAccomodationTypes();
            return result.Data.Select(x => new AccomodationTypeResponse
            {
                Id = x.Id,
                Enum = (int)x.AccomodationType,
                Name = x.Name
            });
        }

        [HttpGet("[action]/{day}")]
        public IEnumerable<TripResponse> Trips(int day)
        {
            var result = _vtServices.GetAllTrips(day);
            return result.Data.Select(x => new TripResponse
            {
                Id = x.Id,
                Name = x.Name,
            });
        }

        [HttpGet("[action]/{id}/{day}")]
        public TripResponse Trip(int id, int day)
        {
            var result = _vtServices.GetTrip(id);

            if (result.Data.Id == 1)
            {
                return new TripResponse
                {
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                };
            }

            var tripAttributes = new List<TripAttributesResponse>();
            foreach (var attribute in result.Data.Attributes)
            {
                tripAttributes.Add(new TripAttributesResponse
                {
                    Enum = attribute.Attribute.Attribute.ToString(),
                    Name = attribute.Attribute.Name
                });
            }

            var spots = new List<SpotsResponse>();
            foreach (var spot in result.Data.Spots.Where(x => (int)x.Day == day))
            {
                spots.Add(new SpotsResponse
                {
                    Day = (int)spot.Day,
                    Spots = spot.Spots
                });
            }

            var remainingSpots = _vtServices.GetRemainingSpots(id, day);

            return new TripResponse
            {
                Id = result.Data.Id,
                Name = result.Data.Name,
                Description = result.Data.Description,
                Length = result.Data.Length,
                Time = result.Data.Time,
                Price = result.Data.Price,
                Elevation = string.IsNullOrWhiteSpace(result.Data.Elevation) ? "-" : result.Data.Elevation,
                Age = string.IsNullOrWhiteSpace(result.Data.Age) ? "minden" : result.Data.Age,
                Attributes = tripAttributes,
                Category = new TripCategoryResponse
                {
                    Name = result.Data.Category.Name,
                    Enum = result.Data.Category.TripCategory.ToString(),
                },
                Difficulty = new TripDifficultyResponse
                {
                    Name = result.Data.Difficulty.Name,
                    Enum = result.Data.Difficulty.TripDifficulty.ToString(),
                },
                Spots = new SpotsResponse
                {
                    Day = (int)result.Data.Spots.FirstOrDefault(x => (int)x.Day == day).Day,
                    Spots = result.Data.Spots.FirstOrDefault(x => (int)x.Day == day).Spots
                },
                RemainingSpots = new SpotsResponse
                {
                    Day = (int)remainingSpots.Data?.Day,
                    Spots = remainingSpots.Data?.Spots - remainingSpots.Data?.Users?.Count() ?? 0,
                }
            };
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserRequest model)
        {
            var result = _vtServices.AddUser(VtUserMapper.ToUser(model));
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetUser(int id)
        {
            var result = _vtServices.GetUser(id);
            return new JsonResult(result.Data);
        }


        public class UserRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public DateTime BirthDate { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Phoneno { get; set; }
            public string Cnp { get; set; }
            public MembershipResponse Member { get; set; }
            public AccomodationTypeResponse Accomodation { get; set; }
            public string CarNo { get; set; }
            public string TagNo { get; set; }
            public string Notes { get; set; }

            public TripResponse Trip1 { get; set; }
            public TripResponse Trip2 { get; set; }
            public TripResponse Trip3 { get; set; }
        }

        public class MembershipResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Enum { get; set; }
        }

        public class AccomodationTypeResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Enum { get; set; }
        }

        public class TripResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Length { get; set; }
            public decimal Time { get; set; }
            public string Age { get; set; }
            public string Elevation { get; set; }
            public decimal Price { get; set; }
            public TripCategoryResponse Category { get; set; }
            public List<TripAttributesResponse> Attributes { get; set; }
            public TripDifficultyResponse Difficulty { get; set; }
            public SpotsResponse Spots { get; set; }
            public SpotsResponse RemainingSpots { get; set; }
        }

        public class TripAttributesResponse
        {
            public string Name { get; set; }
            public string Enum { get; set; }
        }

        public class SpotsResponse
        {
            public int Day { get; set; }
            public int Spots { get; set; }
        }

        public class TripCategoryResponse
        {
            public string Name { get; set; }
            public string Enum { get; set; }
        }

        public class TripDifficultyResponse
        {
            public string Name { get; set; }
            public string Enum { get; set; }
        }

        public class UserResponse
        {
            public string Name { get; set; }
            public string City { get; set; }
            public string Member { get; set; }
        }
    }
}
