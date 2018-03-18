using EKE.Data.Entities.Base;
using System.Collections.Generic;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtYear : IEntityBase
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }

        public virtual List<VtTrip> Trips { get; set; }
    }
}
