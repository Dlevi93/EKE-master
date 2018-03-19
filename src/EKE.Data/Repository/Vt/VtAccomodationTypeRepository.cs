using EKE.Data.Entities.Vandortabor;
using EKE.Data.Repository.Base;

namespace EKE.Data.Repository.Vt
{
    public interface IVtAccomodationTypeRepository : IEntityBaseRepository<VtAccomodationType>
    {
    }

    public class VtAccomodationTypeRepository : EntityBaseRepository<VtAccomodationType>, IVtAccomodationTypeRepository
    {
        public VtAccomodationTypeRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
