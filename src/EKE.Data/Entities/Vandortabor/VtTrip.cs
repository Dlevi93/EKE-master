using System.Collections.Generic;
using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;
using EKE.Data.Entities.Gyopar;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtTrip : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<VtTripAttributes> Attributes { get; set; }
        public VtTripDifficulty Difficulty { get; set; }
        public int Length { get; set; }
        public decimal Price { get; set; }
        public Dictionary<VtDays, int> Spots { get; set; }

        public virtual List<MediaElement> MediaElements { get; set; }
    }
}
