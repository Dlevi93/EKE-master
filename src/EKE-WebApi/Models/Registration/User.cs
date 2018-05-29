using System;
using static EKE_WebApi.Controllers.ValuesController;

namespace EKE_WebApi.Models.Registration
{
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

        public string DayOnlySelected { get; set; }
        public string PaymentCategory { get; set; }

        public TripResponse Trip1 { get; set; }
        public TripResponse Trip2 { get; set; }
        public TripResponse Trip3 { get; set; }
    }
}
