using EKE.Data.Entities.Vandortabor;
using EKE.Data.Infrastructure;
using EKE.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EKE.Service.Services.Vt
{
    public interface IVtServices
    {
        Result<List<VtAccomodationType>> GetAllAccomodationTypes();

        Result<List<VtMembership>> GetAllMemberships();

        Result<List<VtTrip>> GetAllTrips();
    }

    public class VtServices : IVtServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public VtServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result<List<VtAccomodationType>> GetAllAccomodationTypes()
        {
            try
            {
                return new Result<List<VtAccomodationType>>(_unitOfWork.AccomodationTypeRepository.GetAll().ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<VtAccomodationType>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<List<VtMembership>> GetAllMemberships()
        {
            try
            {
                return new Result<List<VtMembership>>(_unitOfWork.MembershipRepository.GetAll().ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<VtMembership>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<List<VtTrip>> GetAllTrips()
        {
            try
            {
                return new Result<List<VtTrip>>(_unitOfWork.TripRepository.GetAllIncluding(x => x.Attributes, x => x.Category, x => x.Difficulty, x => x.MediaElements, x => x.Spots).ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<VtTrip>>(ResultStatus.ERROR, ex.Message);
            }
        }
    }
}
