using EKE.Data.Entities.Enums;
using EKE.Data.Entities.Vandortabor;
using EKE_WebApi.Models.Registration;
using System;
using System.Collections.Generic;

namespace EKE_WebApi.Mappers
{
    public static class VtUserMapper
    {
        public static VtUser ToUser(UserRequest request)
        {
            var accomodationType = new VtAccomodationType
            {
                Id = request.Accomodation.Id,
            };

            var membership = new VtMembership
            {
                Id = request.Member.Id,
            };

            var trips = new List<VtUserSpots>();
            var spot1 = new VtSpot
            {
                Day = VtDays.Tuesday,
                Trip = new VtTrip { Id = request.Trip1?.Id ?? 0 }
            };
            var spot2 = new VtSpot
            {
                Day = VtDays.Wednesday,
                Trip = new VtTrip { Id = request.Trip2?.Id ?? 0 }
            };
            var spot3 = new VtSpot
            {
                Day = VtDays.Thursday,
                Trip = new VtTrip { Id = request.Trip3?.Id ?? 0 }
            };
            trips.Add(new VtUserSpots() { Spot = spot1, SpotId = spot1.Id });
            trips.Add(new VtUserSpots() { Spot = spot2, SpotId = spot2.Id });
            trips.Add(new VtUserSpots() { Spot = spot3, SpotId = spot3.Id });

            return new VtUser
            {
                AccomodationType = accomodationType,
                Birthdate = request.BirthDate,
                Car = request.CarNo,
                City = request.City,
                Cnp = request.Cnp,
                Country = request.Country,
                Email = request.Email,
                Membership = membership,
                MembershipNo = request.TagNo,
                Name = $"{ (object)request.LastName} { (object)request.FirstName}",
                Note = request.Notes,
                PhoneNumber = request.Phoneno,
                RegistrationDate = DateTime.Now,
                Spots = trips
            };
        }
    }
}
