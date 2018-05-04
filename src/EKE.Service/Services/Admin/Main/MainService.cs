using EKE.Data.Entities.Enums;
using EKE.Data.Entities.Gyopar;
using EKE.Data.Entities.Home;
using EKE.Data.Infrastructure;
using EKE.Service.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EKE.Service.Services.Admin.Main
{
    public interface IMainService
    {
        Result AddElement(H_Article model);
        Result<List<H_Article>> GetAllMainArticles();
        Result<H_Article> UpdateCover(ICollection<IFormFile> files, int id);
    }

    public class MainService : IMainService
    {
        private readonly IGeneralService _generalService;
        private readonly IUnitOfWork _unitOfWork;

        public MainService(
            IGeneralService generalService,
            IUnitOfWork unitOfWork)
        {
            _generalService = generalService;
            _unitOfWork = unitOfWork;
        }

        public Result AddElement(H_Article model)
        {
            try
            {
                var result = _unitOfWork.HElementRepository.FindBy(x => x.Title.Trim().ToLower() == model.Title.Trim().ToLower());

                if (result.Any()) return new Result(ResultStatus.ALREADYEXISTS);

                var author = _unitOfWork.AuthorRepository.FindBy(x => x.Name == model.Author.Name).FirstOrDefault();
                model.Author = model.Author.Id == 0 ? new Author { Name = model.Author.Name } : _unitOfWork.AuthorRepository.GetById(model.Author.Id);

                model.MediaElements = _generalService.CreateMediaElements(model.Files, model.DateAdded.Year, "1", ProjectBaseEnum.Main);
                model.Slug = _generalService.GenerateSlug(model.Title, model.DateAdded.Year, "1");

                _unitOfWork.HElementRepository.Add(model);
                _unitOfWork.SaveChanges();

                return new Result(ResultStatus.OK);
            }
            catch (Exception ex)
            {
                return new Result(ResultStatus.OK, ex.Message);
            }
        }

        public Result<List<H_Article>> GetAllMainArticles()
        {
            try
            {
                return new Result<List<H_Article>>(_unitOfWork.HElementRepository.GetAllIncluding(x => x.MediaElements).ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<H_Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<H_Article> UpdateCover(ICollection<IFormFile> files, int id)
        {
            var result = _unitOfWork.HElementRepository.GetByIdIncluding(id, x => x.MediaElements);

            if (result == null) return new Result<H_Article>(ResultStatus.NOT_FOUND);

            if (files == null || files.Count == 0) return new Result<H_Article>(ResultStatus.ERROR);

            result.MediaElements = _generalService.CreateMediaElements(files, result.DateAdded.Year, "1", ProjectBaseEnum.Main);

            _unitOfWork.HElementRepository.Update(result);
            _unitOfWork.SaveChanges();
            return new Result<H_Article>(result);
        }
    }
}
