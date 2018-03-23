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
        Result<VtTrip> GetTrip(int id);

        Result AddUser(VtUser user);
        Result<VtUser> GetUser(int id);
    }

    public class VtServices : IVtServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public VtServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result AddUser(VtUser user)
        {
            try
            {
                var spots = new List<VtUserSpots>();
                foreach (var trip in user.Spots)
                {
                    var spot = _unitOfWork.SpotRepository.GetAllIncludingPred(x => x.Day == trip.Spot.Day && x.Trip.Id == trip.Spot.Trip.Id, y => y.Trip).FirstOrDefault();
                    if (spot != null) spots.Add(new VtUserSpots { Spot = spot, SpotId = spot.Id });
                }

                user.AccomodationType = _unitOfWork.AccomodationTypeRepository.GetById(user.AccomodationType.Id);
                user.Membership = _unitOfWork.MembershipRepository.GetById(user.Membership.Id);
                user.Spots = spots;
                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.SaveChanges();
                return new Result(ResultStatus.OK);
            }
            catch (Exception ex)
            {
                return new Result(ResultStatus.ERROR, ex.Message);
            }
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
                return new Result<List<VtTrip>>(_unitOfWork.TripRepository.GetAllIncluding().ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<VtTrip>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<VtTrip> GetTrip(int id)
        {
            try
            {
                return new Result<VtTrip>(_unitOfWork.TripRepository.GetByIdIncluding(id, x => x.Attributes, x => x.Category, x => x.Difficulty, x => x.MediaElements, x => x.Spots));
            }
            catch (Exception ex)
            {
                return new Result<VtTrip>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<VtUser> GetUser(int id)
        {
            try
            {
                return new Result<VtUser>(_unitOfWork.UserRepository.GetByIdIncluding(id, x => x.Spots, x => x.Membership, x => x.AccomodationType));
            }
            catch (Exception ex)
            {
                return new Result<VtUser>(ResultStatus.ERROR, ex.Message);
            }
        }
    }
}
