using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtTripDifficulty : IEntityBase
    {
        public int Id { get; set; }
        public VtTripDifficulties TripDifficulty { get; set; }
        public string Name { get; set; }
    }
}
