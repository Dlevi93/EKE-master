using System;

namespace EKE_WebApi.Models.Registration
{
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
        public string Trips { get; set; }
    }

    public class UserResponseToVt
    {
        public string Name { get; set; }
        public string Birthdate { get; set; }
        public string RegistrationDate { get; set; }
        public string Member { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Cnp { get; set; }
        public string Car { get; set; }
        public string Note { get; set; }

        public string PaymentCategory { get; set; }
        public string SelectedDay { get; set; }


        public string Membership { get; set; }
        public string AccomodationType { get; set; }

        public string Trips { get; set; }
    }
}
