using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtSpot : IEntityBase
    {
        public int Id { get; set; }
        public VtDays Day { get; set; }
        public int Spots { get; set; }

        public virtual int TripId { get; set; }
        public virtual VtTrip Trip { get; set; }

        public virtual IEnumerable<VtUserSpots> Users { get; set; }
    }
}
