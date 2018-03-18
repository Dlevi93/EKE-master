using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace EKE_Vandortabor.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<Membership> Memberships()
        {
            var rng = new Random();
            return Enumerable.Range(1, 9).Select(index => new Membership
            {
                Id = 1,
                Enum = 1,
                Name = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<AccomodationType> AccomodationTypes()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new AccomodationType
            {
                Id = 1,
                Enum = 1,
                Name = Summaries[rng.Next(Summaries.Length)]
            });
        }


        [HttpPost]
        public IActionResult Add([FromBody] AddUser model)
        {
            return null;
        }


        public class Membership
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Enum { get; set; }
        }

        public class AccomodationType
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Enum { get; set; }
        }

        public class AddUser
        {
            public string name { get; set; }
            public DateTime birthdate { get; set; }
        }

    }
}
