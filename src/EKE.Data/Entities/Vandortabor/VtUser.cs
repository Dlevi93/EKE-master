using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;
using System;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtUser : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public VtMember Membership { get; set; }
        public int MembershipNo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Cnp { get; set; }
        public VtAccomodationType AccomodationType { get; set; }
        public string Car { get; set; }
        public string Note { get; set; }
    }
}
