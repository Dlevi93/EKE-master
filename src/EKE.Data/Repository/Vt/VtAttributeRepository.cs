using EKE.Data.Entities.Vandortabor;
using EKE.Data.Repository.Base;

namespace EKE.Data.Repository.Vt
{
    public interface IVtAttributeRepository : IEntityBaseRepository<VtAttribute>
    {

    }

    public class VtAttributeRepository : EntityBaseRepository<VtAttribute>, IVtAttributeRepository
    {
        public VtAttributeRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
