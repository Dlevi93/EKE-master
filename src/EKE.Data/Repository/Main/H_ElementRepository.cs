using EKE.Data.Entities.Home;
using EKE.Data.Repository.Base;

namespace EKE.Data.Repository.Main
{
    public interface IHElementRepository : IEntityBaseRepository<H_Article>
    {
    }

    public class H_ElementRepository : EntityBaseRepository<H_Article>, IHElementRepository
    {
        public H_ElementRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
    }
}