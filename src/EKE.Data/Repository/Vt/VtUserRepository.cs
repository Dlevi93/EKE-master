using EKE.Data.Entities.Vandortabor;
using EKE.Data.Repository.Base;

namespace EKE.Data.Repository.Vt
{
    public interface IVtUserRepository : IEntityBaseRepository<VtUser>
    {

    }

    public class VtUserRepository : EntityBaseRepository<VtUser>, IVtUserRepository
    {
        public VtUserRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
