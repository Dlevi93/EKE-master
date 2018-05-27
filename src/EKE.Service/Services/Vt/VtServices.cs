using EKE.Data.Entities.Vandortabor;
using EKE.Data.Infrastructure;
using EKE.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using EKE.Data.Entities.Enums;

namespace EKE.Service.Services.Vt
{
    public interface IVtServices
    {
        Result<List<VtAccomodationType>> GetAllAccomodationTypes();

        Result<List<VtMembership>> GetAllMemberships();

        Result<List<VtTrip>> GetAllTrips(int day);
        Result<List<VtTrip>> GetAllTripsForTable();
        Result<VtTrip> GetTrip(int id);

        Result AddUser(VtUser user);
        Result<VtUser> GetUser(int id);
        Result<List<VtUser>> GetAllUsers();

        Result<VtSpot> GetRemainingSpots(int tripId, int day);

        string GetTripNames(List<VtUserSpots> spots, List<VtTrip> trips);
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

        public Result<List<VtTrip>> GetAllTrips(int day)
        {
            try
            {
                return new Result<List<VtTrip>>(_unitOfWork.SpotRepository.GetAllIncludingPred(x => (int)x.Day == day, x => x.Trip).Select(x => x.Trip).ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<VtTrip>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<List<VtTrip>> GetAllTripsForTable()
        {
            try
            {
                return new Result<List<VtTrip>>(_unitOfWork.SpotRepository.GetAllIncluding(x => x.Trip).Select(x => x.Trip).ToList());
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
                var trip = _unitOfWork.TripRepository.GetByIdIncluding(id, x => x.Attributes, x => x.Category, x => x.Difficulty, x => x.MediaElements, x => x.Spots);

                foreach (var attribute in trip.Attributes)
                {
                    attribute.Attribute = _unitOfWork.AttributeRepository.GetById(attribute.AttributeId);
                }

                return new Result<VtTrip>(trip);
            }
            catch (Exception ex)
            {
                return new Result<VtTrip>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<VtSpot> GetRemainingSpots(int tripId, int day)
        {
            try
            {
                var allSpots = _unitOfWork.SpotRepository.GetAllIncludingPred(x => (int)x.Day == day && x.TripId == tripId, x => x.Users).FirstOrDefault();

                return new Result<VtSpot>(allSpots);
            }
            catch (Exception ex)
            {
                return new Result<VtSpot>(ResultStatus.ERROR, ex.Message);
            }
        }

        public string GetTripNames(List<VtUserSpots> spots, List<VtTrip> trips)
        {
            var tripString = "";
            decimal tripPrice = 0;
            try
            {
                var tripId = spots.FirstOrDefault(x => x.Spot.Day == VtDays.Tuesday).Spot.TripId;
                var trip = trips.FirstOrDefault(x => x.Id == tripId);
                tripString += "1. " + trip?.Name + "<br />";
                tripPrice += trip?.Price ?? 0;
                tripId = spots.FirstOrDefault(x => x.Spot.Day == VtDays.Wednesday).Spot.TripId;
                trip = trips.FirstOrDefault(x => x.Id == tripId);
                tripString += "2. " + trip?.Name + "<br />";
                tripPrice += trip?.Price ?? 0;
                tripId = spots.FirstOrDefault(x => x.Spot.Day == VtDays.Thursday).Spot.TripId;
                trip = trips.FirstOrDefault(x => x.Id == tripId);
                tripString += "3. " + trip?.Name + "<br />";
                tripPrice += trip?.Price ?? 0;
                return $"{tripString} Végösszeg: {tripPrice}";
            }
            catch (Exception)
            {
                return "-Hiba-";
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

        public Result<List<VtUser>> GetAllUsers()
        {
            try
            {
                return new Result<List<VtUser>>(_unitOfWork.UserRepository.GetAllIncluding(x => x.Membership, x => x.Spots).ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<VtUser>>(ResultStatus.ERROR, ex.Message);
            }
        }
    }
}
