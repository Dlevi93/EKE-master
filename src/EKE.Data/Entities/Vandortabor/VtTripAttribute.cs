using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtTripAttribute : IEntityBase
    {
        public int Id { get; set; }
        public VtTripAttributes Attribute { get; set; }
        public string Name { get; set; }
    }
}
