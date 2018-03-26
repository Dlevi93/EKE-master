using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;
using System.Collections.Generic;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtAttribute : IEntityBase
    {
        public int Id { get; set; }
        public VtTripAttributes Attribute { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<VtTripToAttributes> Trips { get; set; }
    }
}
