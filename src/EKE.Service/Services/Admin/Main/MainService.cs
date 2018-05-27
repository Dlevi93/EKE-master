using EKE.Data.Entities.Enums;
using EKE.Data.Entities.Gyopar;
using EKE.Data.Entities.Home;
using EKE.Data.Infrastructure;
using EKE.Service.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EKE.Service.Services.Admin.Main
{
    public interface IMainService
    {
        Result AddElement(H_Article model);
        Result<List<H_Article>> GetAllMainArticles();
        Result<H_Article> UpdateCover(ICollection<IFormFile> files, int id);

        Result<List<H_Article>> GetAllElements();
        Result<List<H_Article>> GetAllArticles();
        Result<List<H_Article>> GetAllEvents();
        Result<List<H_Article>> GetFutureEvents();
        Result<List<H_Article>> GetLatestElements();
        Result<List<H_Article>> GetLatestArticles();
        Result<List<H_Article>> GetNewsElements();
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

                if (model.Files != null) model.MediaElements = _generalService.CreateMediaElements(model.Files, model.DateAdded.Year, "1", ProjectBaseEnum.Main);
                model.Slug = _generalService.GenerateSlug(model.Title, model.DateAdded.Year, "1");

                _unitOfWork.HElementRepository.Add(model);
                _unitOfWork.SaveChanges();

                return new Result(ResultStatus.OK);
            }
            catch (Exception ex)
            {
                return new Result(ResultStatus.EXCEPTION, ex.Message);
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

        public Result<List<H_Article>> GetAllArticles()
        {
            try
            {
                return new Result<List<H_Article>>(_unitOfWork.HElementRepository.GetAllIncludingPred(x => x.Category == Data.Entities.Enums.NewsCategories.Hír, x => x.MediaElements).ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<H_Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<List<H_Article>> GetAllElements()
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

        public Result<List<H_Article>> GetLatestElements()
        {
            try
            {
                return new Result<List<H_Article>>(_unitOfWork.HElementRepository.GetAll().OrderByDescending(x => x.DateAdded).Take(5).ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<H_Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<List<H_Article>> GetAllEvents()
        {
            try
            {
                return new Result<List<H_Article>>(_unitOfWork.HElementRepository.GetAllIncludingPred(x => x.Category == Data.Entities.Enums.NewsCategories.Esemény, x => x.MediaElements).ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<H_Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<List<H_Article>> GetFutureEvents()
        {
            try
            {
                return new Result<List<H_Article>>(_unitOfWork.HElementRepository.GetAllIncludingPred(x => x.EventStartDate > DateTime.Now).OrderBy(x => x.EventStartDate).Take(5).ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<H_Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<List<H_Article>> GetLatestArticles()
        {
            try
            {
                var result = _unitOfWork.HElementRepository.GetAllIncludingPred(x => x.Category == NewsCategories.Hír, x => x.MediaElements).OrderByDescending(x => x.DateAdded).Take(12).ToList();

                foreach (var article in result)
                {
                    article.MediaElements = SolveMediaElements(article);
                }

                return new Result<List<H_Article>>(result);


            }
            catch (Exception ex)
            {
                return new Result<List<H_Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public List<MediaElement> SolveMediaElements(H_Article article)
        {
            if (!article.MediaElements.Any())
            {
                return new List<MediaElement>()
                {
                    new MediaElement()
                    {
                        OriginalName = "Uploads/2018/1/r1_7CRBGH1C5Y_download (1).jpg",
                        Name = "7CRBGH1C5Y_download(1).jpg"
                    }
                };
            }
            return article.MediaElements.ToList();
        }

        public Result<List<H_Article>> GetNewsElements()
        {
            try
            {
                var result = _unitOfWork.HElementRepository.GetAllIncludingPred(x => x.Category == NewsCategories.Hír, x => x.MediaElements).OrderByDescending(x => x.DateAdded).Skip(12).Take(3).ToList();

                foreach (var article in result)
                {
                    article.MediaElements = SolveMediaElements(article);
                    article.Description = StripHtml(article.Description);
                }

                return new Result<List<H_Article>>(result);
            }
            catch (Exception ex)
            {
                return new Result<List<H_Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty).Substring(0, 150);
        }
    }
}
