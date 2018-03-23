using EKE.Data.Entities.Vandortabor;
using EKE.Data.Repository.Base;

namespace EKE.Data.Repository.Vt
{
    public interface IVtSpotRepository : IEntityBaseRepository<VtSpot>
    {

    }

    public class VtSpotRepository : EntityBaseRepository<VtSpot>, IVtSpotRepository
    {
        public VtSpotRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
