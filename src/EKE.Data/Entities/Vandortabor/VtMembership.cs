using EKE.Data.Entities.Base;
using EKE.Data.Entities.Enums;

namespace EKE.Data.Entities.Vandortabor
{
    public class VtMembership : IEntityBase
    {
        public int Id { get; set; }
        public VtMember Membership { get; set; }
        public string Name { get; set; }
    }
}
