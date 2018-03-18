using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtAccomodationType : IEntityBase
    {
        public int Id { get; set; }
        public VtAccomodationTypes AccomodationType { get; set; }
        public string Name { get; set; }
    }
}
