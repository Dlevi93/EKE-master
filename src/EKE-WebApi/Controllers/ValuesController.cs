﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using EKE.Data.Entities.Consts;
using EKE.Data.Entities.Enums;
using EKE.Data.Entities.Vandortabor;
using EKE.Service.Services.Vt;
using EKE_WebApi.Mappers;
using EKE_WebApi.Models.Registration;
using Microsoft.AspNetCore.Mvc;

namespace EKE_WebApi.Controllers
{
    [Produces("application/json")]
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
            var trips = _vtServices.GetAllTripsForTable();

            return result.Data.Select(x => new UserResponse
            {
                Name = x.Name,
                City = x.City,
                Member = x.Membership?.Name.ToString() ?? "-",
                Trips = _vtServices.GetTripNames(x.Spots.ToList(), trips.Data, x.PaymentCategory, !string.IsNullOrEmpty(x.Car))
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<UserResponseToVt> UserListToTb()
        {
            var result = _vtServices.GetAllUsers();
            var trips = _vtServices.GetAllTripsForTable();

            return result.Data.Select(x => new UserResponseToVt
            {
                Name = x.Name,
                Birthdate = x.Birthdate.ToString("yyyy/dd/MM"),
                RegistrationDate = x.RegistrationDate.ToString("yyyy/dd/MM HH:mm:ss"),
                City = x.City,
                Country = x.Country,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Cnp = x.Cnp,
                Car = x.Car,
                Note = x.Note,
                PaymentCategory = x.PaymentCategory.ToString(),
                SelectedDay = x.SelectedDay,
                AccomodationType = x.AccomodationType.AccomodationType.ToString(),
                AccomodationTypeName = x.AccomodationType.Name,
                Member = x.Membership?.Name.ToString() ?? "-",
                Trips = _vtServices.GetTripNames(x.Spots.ToList(), trips.Data, x.PaymentCategory, !string.IsNullOrEmpty(x.Car))
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
        public HttpResponseMessage Add([FromBody] UserRequest model)
        {
            var vtUser = VtUserMapper.ToUser(model);

            foreach (var item in vtUser.Spots)
            {
                var remainingSpots = _vtServices.GetRemainingSpots(item.Spot.Trip.Id, (int)item.Spot.Day);
                if (remainingSpots.IsOk())
                {
                    var remaining = remainingSpots.Data?.Spots - remainingSpots.Data?.Users?.Count() ?? 0;
                    if (remaining < 1)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                    }
                }
            }

            var result = _vtServices.AddUser(vtUser);
            if (result.IsOk())
                return new HttpResponseMessage(HttpStatusCode.OK);

            return new HttpResponseMessage(HttpStatusCode.NotModified);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetUser(int id)
        {
            var result = _vtServices.GetUser(id);
            return new JsonResult(result.Data);
        }
    }
}
