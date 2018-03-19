using EKE.Data.Entities.Vandortabor;
using EKE.Data.Repository.Base;

namespace EKE.Data.Repository.Vt
{
    public interface IVtMembershipRepository : IEntityBaseRepository<VtMembership>
    {
    }

    public class VtMembershipRepository : EntityBaseRepository<VtMembership>, IVtMembershipRepository
    {
        public VtMembershipRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
