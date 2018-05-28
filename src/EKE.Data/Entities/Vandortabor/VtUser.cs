using EKE.Data.Entities.Base;
using System;
using System.Collections.Generic;
using EKE.Data.Entities.Enums;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtUser : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string MembershipNo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Cnp { get; set; }
        public string Car { get; set; }
        public string Note { get; set; }

        public VtPaymentCategory PaymentCategory { get; set; }
        public string SelectedDay { get; set; }


        public virtual VtMembership Membership { get; set; }
        public virtual VtAccomodationType AccomodationType { get; set; }

        public virtual IEnumerable<VtUserSpots> Spots { get; set; }
    }

    public class VtUserSpots
    {
        public int UserId { get; set; }
        public VtUser User { get; set; }

        public int SpotId { get; set; }
        public VtSpot Spot { get; set; }
    }
}
