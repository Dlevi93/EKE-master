using EKE.Data.Entities.Vandortabor;
using EKE.Data.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EKE.Data.Repository.Vt
{
    public interface IVtTripRepository : IEntityBaseRepository<VtTrip>
    {

    }

    public class VtTripRepository : EntityBaseRepository<VtTrip>, IVtTripRepository
    {
        public VtTripRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
