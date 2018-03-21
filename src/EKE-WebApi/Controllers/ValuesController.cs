using System.Collections.Generic;
using System.Linq;
using EKE.Service.Services.Vt;
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

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

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

        [HttpGet("[action]")]
        public IEnumerable<TripResponse> Trips()
        {
            var result = _vtServices.GetAllTrips();
            return result.Data.Select(x => new TripResponse
            {
                Id = x.Id,
                Name = x.Name,
            });
        }

        [HttpGet("[action]/{id}")]
        public TripResponse Trip(int id)
        {
            var result = _vtServices.GetTrip(id);
            var tripAttributes = new List<TripAttributesResponse>();
            foreach (var attribute in result.Data.Attributes)
            {
                tripAttributes.Add(new TripAttributesResponse
                {
                    EnumId = (int)attribute.Attribute,
                    Name = attribute.Name
                });
            }

            return new TripResponse
            {
                Id = result.Data.Id,
                Name = result.Data.Name,
                Description = result.Data.Description,
                Length = result.Data.Length,
                Price = result.Data.Price,
                Attributes = tripAttributes,
                Category = new TripCategoryResponse
                {
                    Name = result.Data.Category.Name,
                    EnumId = (int)result.Data.Category.TripCategory,
                },
                Difficulty = new TripDifficultyResponse
                {
                    Name = result.Data.Difficulty.Name,
                    EnumId = (int)result.Data.Difficulty.TripDifficulty,
                }
            };
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
            public int Length { get; set; }
            public decimal Price { get; set; }
            public TripCategoryResponse Category { get; set; }
            public List<TripAttributesResponse> Attributes { get; set; }
            public TripDifficultyResponse Difficulty { get; set; }
        }

        public class TripAttributesResponse
        {
            public string Name { get; set; }
            public int EnumId { get; set; } 
        }

        public class TripCategoryResponse
        {
            public string Name { get; set; }
            public int EnumId { get; set; }
        }

        public class TripDifficultyResponse
        {
            public string Name { get; set; }
            public int EnumId { get; set; }
        }
    }
}
