using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtTripCategory : IEntityBase
    {
        public int Id { get; set; }
        public VtTripCategories TripCategory { get; set; }
        public string Name { get; set; }
    }
}
