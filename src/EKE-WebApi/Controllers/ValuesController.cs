using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Description = x.Description,
                Length = x.Length,
                Price = x.Price
            });
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
        }
    }
}
