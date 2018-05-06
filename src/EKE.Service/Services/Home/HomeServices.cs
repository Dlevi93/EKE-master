using EKE.Data.Entities.Home;
using EKE.Data.Infrastructure;
using EKE.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EKE.Service.Services.Home
{
    public interface IHomeServices
    {
    }

    public class HomeServices : IHomeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
