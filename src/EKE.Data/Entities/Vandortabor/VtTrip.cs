using System.Collections.Generic;
using EKE.Data.Entities.Base;
using EKE.Data.Entities.Gyopar;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtTrip : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Length { get; set; }
        public decimal Time { get; set; }
        public string Age { get; set; }
        public string Elevation { get; set; }
        public decimal Price { get; set; }

        public virtual VtTripDifficulty Difficulty { get; set; }
        public virtual VtTripCategory Category { get; set; }


        public virtual List<VtSpot> Spots { get; set; }

        public virtual IEnumerable<VtTripToAttributes> Attributes { get; set; }
        public virtual IEnumerable<MediaElement> MediaElements { get; set; }

        public virtual IEnumerable<VtUser> Users { get; set; }
    }

    public class VtTripToAttributes
    {
        public int TripId { get; set; }
        public VtTrip Trip { get; set; }

        public int AttributeId { get; set; }
        public VtAttribute Attribute { get; set; }
    }
}
