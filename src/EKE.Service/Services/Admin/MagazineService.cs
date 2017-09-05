﻿using EKE.Data.Entities.Gyopar;
using EKE.Data.Infrastructure;
using EKE.Data.Repository;
using EKE.Service.ServiceModel;
using EKE.Service.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using ImageMagick;
using EKE.Data.Entities.Enums;

namespace EKE.Service.Services.Admin
{
    public interface IMagazineService : IBaseService
    {
        Result<List<Magazine>> GetAllMagazines();
        Result<List<Magazine>> GetAllMagazinesIncluding();
        Result<List<Magazine>> GetLastMagazines(int count);
        Result<List<Magazine>> GetAllMagazinesBy(Expression<Func<Magazine, bool>> predicate);

        Result<Magazine> GetMagazineById(int id);
        Result<Magazine> Add(Magazine model);
        Result<Magazine> Update(Magazine model);
        Result<Magazine> Update(XEditSM model);
        Result<Magazine> UpdateCover(IFormFile files, int id);
        Result<Magazine> GetMagazinesBy(Expression<Func<Magazine, bool>> predicate);

        List<MediaElement> CreateMediaElements(IFormFile files, int year, string section);

        Result<List<Article>> GetAllArticles();
        Result<List<Article>> GetAllArticlesBy(Expression<Func<Article, bool>> predicate);
        Result<Article> GetArticleBySlug(string slug);
        Result<Article> GetArticleById(int id);
        Result<Article> Add(Article model, string userName);
        Result<Article> Update(Article model, string username);
        Result DeleteArticle(int id);

        Result<MagazineCategory> Add(MagazineCategory model);
        Result<List<MagazineCategory>> GetAllMagazineCategories();
        Result<MagazineCategory> GetMagazineCategoryById(int id);

        Result<bool> DeleteMagazineCategory(int id);
        Result<bool> DeleteMagazine(int id);

        Result<List<Tag>> GetAllTags();
        Result<Tag> Add(Tag model);
        Result DeleteTag(int id);

        Result<List<Author>> GetAllAuthors();
        Result FormatHtml();
    }

    public class MagazineService : BaseService, IMagazineService
    {
        private readonly IEntityBaseRepository<Magazine> _magazineRepo;
        private readonly IEntityBaseRepository<Article> _articleRepo;
        private readonly IEntityBaseRepository<MagazineCategory> _magazineCatRepo;
        private readonly IEntityBaseRepository<Tag> _tagRepo;
        private readonly IEntityBaseRepository<MediaElement> _mediaElementRepo;
        private readonly IEntityBaseRepository<Author> _authorRepo;
        private readonly IHostingEnvironment _environment;

        public MagazineService(
            IEntityBaseRepository<Magazine> magazineRepository,
            IEntityBaseRepository<Article> articleRepository,
            IEntityBaseRepository<MagazineCategory> magazineCatRepository,
            IEntityBaseRepository<Tag> tagRepository,
            IEntityBaseRepository<MediaElement> mediaElementRepository,
            IEntityBaseRepository<Author> authorRepository,
            IHostingEnvironment environment,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _magazineRepo = magazineRepository;
            _articleRepo = articleRepository;
            _magazineCatRepo = magazineCatRepository;
            _tagRepo = tagRepository;
            _mediaElementRepo = mediaElementRepository;
            _authorRepo = authorRepository;
            _environment = environment;
        }

        #region Magazines
        #region CRUD
        public Result<List<Magazine>> GetAllMagazines()
        {
            return new Result<List<Magazine>>(_magazineRepo.GetAll().ToList());
        }

        public Result<List<Magazine>> GetAllMagazinesIncluding()
        {
            return new Result<List<Magazine>>(_magazineRepo.GetAllIncluding(x => x.Articles, x => x.Category, x => x.MediaElements).ToList());
        }

        public Result<List<Magazine>> GetLastMagazines(int count)
        {
            try
            {
                var result = _magazineRepo.GetAllIncludingPred(x => x.Visible, x => x.Articles, x => x.Category, x => x.MediaElements).OrderByDescending(x => x.DateCreated).Take(count).ToList();
                CheckMediaElements(result);
                return new Result<List<Magazine>>(result);
            }
            catch (Exception ex)
            {
                return new Result<List<Magazine>>(ResultStatus.EXCEPTION, ex.Message);
            }
        }

        public Result<List<Magazine>> GetAllMagazinesBy(Expression<Func<Magazine, bool>> predicate)
        {
            try
            {
                var result = _magazineRepo.GetAllIncludingPred(predicate, x => x.Author, x => x.Articles, x => x.MediaElements).ToList();
                CheckMediaElements(result);
                return new Result<List<Magazine>>(result);
            }
            catch (Exception ex)
            {
                return new Result<List<Magazine>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<Magazine> GetMagazineById(int id)
        {
            try
            {
                var result = _magazineRepo.GetByIdIncluding(id, x => x.Articles, x => x.MediaElements);
                CheckMediaElements(result);
                return new Result<Magazine>(result);
            }
            catch (Exception ex)
            {
                return new Result<Magazine>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<Magazine> Add(Magazine model)
        {
            try
            {

                var category = _magazineCatRepo.GetById(model.Category.Id);
                if (category == null)
                    return new Result<Magazine>(ResultStatus.ERROR, "Hiba a kategória lekérése során");

                model.Category = category;

                var exists = _magazineRepo.FindBy(x => x.PublishYear == model.PublishYear && x.PublishSection.Contains(model.PublishSection) && x.Category.Id == model.Category.Id);
                if (exists.Any())
                    return new Result<Magazine>(ResultStatus.ALREADYEXISTS, "A lapszám már létezik! Kérem ellenőrizze az adatokat!");

                if (model.Files != null)
                {
                    model.MediaElements = CreateMediaElements(model.Files, model.PublishYear, model.PublishSection);
                }

                model.DateCreated = DateTime.Now;
                model.Slug = GenerateSlug(model.Title, model.PublishYear, model.PublishSection);
                _magazineRepo.Add(model);
                SaveChanges();
                return new Result<Magazine>(model);
            }
            catch (Exception ex)
            {
                return new Result<Magazine>(ResultStatus.EXCEPTION, ex.Message);
            }
        }

        public Result<Magazine> Update(Magazine model)
        {
            _magazineRepo.Update(model);
            SaveChanges();
            return new Result<Magazine>(model);
        }

        public Result<bool> DeleteMagazine(int id)
        {
            try
            {
                var magazine = _magazineRepo.GetByIdIncluding(id, x => x.MediaElements, x => x.Articles);
                _magazineRepo.Delete(magazine);
                SaveChanges();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                return new Result<bool>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<Magazine> GetMagazinesBy(Expression<Func<Magazine, bool>> predicate)
        {
            try
            {
                var result = _magazineRepo.GetAllIncludingPred(predicate, x => x.Author, x => x.Articles).FirstOrDefault();
                return new Result<Magazine>(result);
            }
            catch (Exception ex)
            {
                return new Result<Magazine>(ResultStatus.ERROR, ex.Message);
            }
        }
        #endregion
        #endregion

        #region MediaElements
        public List<MediaElement> CreateMediaElements(IFormFile files, int year, string section)
        {
#warning MEGOLDANI AZ EGESZ METODUST ATNEZNI!
            var gyoparPath = _environment.WebRootPath.Replace("EKE-Admin.Web", "EKE-Gyopar.Web");
            var relativePath = String.Format("Uploads/{0}/{1}", year.ToString().Trim(), section.Trim());
            var uploads = Path.Combine(gyoparPath, relativePath);
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var mediaElements = new List<MediaElement>();
            if (files.Length > 0)
            {
                var uploadPath = Path.Combine(uploads, files.FileName);
                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    files.CopyTo(fileStream);
                }
                var fileName = String.Format("r1_{0}", files.FileName);
                var outputPath = Path.Combine(uploads, fileName);
                var resize = ResizeImage(uploadPath, outputPath, MediaTypesScope.Cover);

                var mediaElem = new MediaElement();
                mediaElem.OriginalName = String.Format("{0}/{1}", relativePath, resize.IsOk() ? fileName : files.FileName);
                mediaElem.Description = string.Format("{0}_{1}", year.ToString().Trim(), section.Trim());
                mediaElem.Name = RandomString(10);
                mediaElem.Type = MediaTypesEnum.Image;
                mediaElem.Scope = MediaTypesScope.Cover;
                mediaElements.Add(mediaElem);
            }

            return mediaElements;
        }

        public Result ResizeImage(string inputPath, string outputPath, MediaTypesScope type)
        {
            var size = 0;
            var quality = 0;

            switch (type)
            {
                case MediaTypesScope.Background:
                    break;
                case MediaTypesScope.Cover:
                    size = 400;
                    quality = 75;
                    break;
                case MediaTypesScope.Article:
                    break;
                default:
                    break;
            }

            try
            {
                using (var image = new MagickImage(inputPath))
                {
                    image.Resize(size, size);
                    image.Strip();
                    image.Quality = quality;
                    image.Write(outputPath);
                    return new Result(ResultStatus.OK);
                }
            }
            catch (Exception)
            {
                return new Result(ResultStatus.EXCEPTION);
            }
        }
        #endregion

        #region Articles
        #region CRUD
        public Result<List<Article>> GetAllArticles()
        {
            try
            {
                return new Result<List<Article>>(_articleRepo.GetAll().ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<List<Article>> GetAllArticlesBy(Expression<Func<Article, bool>> predicate)
        {
            try
            {
                var result = _articleRepo.GetAllIncludingPred(predicate, x => x.Author, x => x.Magazine).ToList();
                return new Result<List<Article>>(result);
            }
            catch (Exception ex)
            {
                return new Result<List<Article>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<Article> GetArticleById(int id)
        {
            try
            {
                var result = _articleRepo.GetByIdIncluding(id, x => x.Author, x => x.MediaElement, x => x.ArticleTag, x => x.Magazine, x => x.Magazine.Category);
                if (result == null) return new Result<Article>(ResultStatus.NOT_FOUND);
                return new Result<Article>(result);
            }
            catch (Exception ex)
            {
                return new Result<Article>(ResultStatus.EXCEPTION, ex.Message);
            }
        }

        public Result<Article> GetArticleBySlug(string slug)
        {
            try
            {
                var result = _articleRepo.GetAllIncludingPred(x => x.Slug == slug, x => x.Author, x => x.MediaElement, x => x.ArticleTag, x => x.Magazine, x => x.Magazine.Category).FirstOrDefault();
                if (result == null) return new Result<Article>(ResultStatus.NOT_FOUND);
                return new Result<Article>(result);
            }
            catch (Exception ex)
            {
                return new Result<Article>(ResultStatus.EXCEPTION, ex.Message);
            }
        }

        public Result<Article> Add(Article model, string userName)
        {
            try
            {
                if (model.Files != null)
                {
                    var uploads = Path.Combine(_environment.WebRootPath, String.Format("Uploads/{0}/{1}", model.Magazine.PublishYear, model.Magazine.PublishSection));
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);

                    var mediaElements = new List<MediaElement>();
                    foreach (var file in model.Files)
                    {
                        if (file.Length > 0)
                        {
                            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                file.CopyToAsync(fileStream);
                            }
                        }
                        var mediaElem = new MediaElement();
                        mediaElem.OriginalName = String.Format("{0}/{1}", uploads, file.Name);
                        mediaElem.Name = RandomString(10);
                        mediaElem.Type = Data.Entities.Enums.MediaTypesEnum.Image;
                        mediaElements.Add(mediaElem);
                    }

                    model.MediaElement = mediaElements;
                }

                var magCat = _magazineCatRepo.GetById(model.Magazine.Category.Id);
                if (magCat == null)
                    return new Result<Article>(ResultStatus.NOT_FOUND, "Folyóirat nem található");

                var magazine = _magazineRepo.FindBy(x => x.PublishYear == model.Magazine.PublishYear && x.PublishSection.Contains(model.Magazine.PublishSection) && x.Category.Id == model.Magazine.Category.Id);
                if (!magazine.Any())
                {
                    model.Magazine.Category = magCat;
                    model.Magazine.Title = String.Format("{0} / {1}", model.Magazine.PublishYear, model.Magazine.PublishSection);
                    model.Magazine.Slug = GenerateSlug(model.Magazine.Title, model.Magazine.PublishYear, model.Magazine.PublishSection);
                    model.Magazine.DateCreated = DateTime.Now;
                }
                else
                {
                    model.Magazine = magazine.FirstOrDefault();
                }

                var author = new Author();
                if (model.Author.Id == 0)
                {
                    author = new Author { Name = model.Author.Name };
                }
                else
                {
                    author = _authorRepo.GetById(model.Author.Id);
                }
                model.Author = author;
                model.Slug = GenerateSlug(model.Title, model.Magazine.PublishYear, model.Magazine.PublishSection);
                model.PublishedBy = userName;
                model.DateCreated = DateTime.Now;

                //foreach (var item in model.ArticleTags)
                //{
                //    var tag = _tagRepo.GetById(Convert.ToInt32(item));
                //}

                _articleRepo.Add(model);
                SaveChanges();
                return new Result<Article>(model);
            }
            catch (Exception ex)
            {
                return new Result<Article>(ResultStatus.ERROR, ex.Message);
            }

        }

        public Result<Article> Update(Article model, string username)
        {
            try
            {
                if (model.Files != null)
                {
                    var uploads = Path.Combine(_environment.WebRootPath, String.Format("Uploads/{0}/{1}", model.Magazine.PublishYear, model.Magazine.PublishSection));
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);

                    var mediaElements = new List<MediaElement>();
                    foreach (var file in model.Files)
                    {
                        if (file.Length > 0)
                        {
                            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                file.CopyToAsync(fileStream);
                            }
                        }
                        var mediaElem = new MediaElement();
                        mediaElem.OriginalName = String.Format("{0}/{1}", uploads, file.Name);
                        mediaElem.Name = RandomString(10);
                        mediaElem.Type = Data.Entities.Enums.MediaTypesEnum.Image;
                        mediaElements.Add(mediaElem);
                    }

                    model.MediaElement = mediaElements;
                }

                var magCat = _magazineCatRepo.GetById(model.Magazine.Category.Id);
                if (magCat == null)
                    return new Result<Article>(ResultStatus.NOT_FOUND, "Folyóirat nem található");

                var magazine = _magazineRepo.FindBy(x => x.PublishYear == model.Magazine.PublishYear && x.PublishSection.Contains(model.Magazine.PublishSection) && x.Category.Id == model.Magazine.Category.Id);
                if (!magazine.Any())
                {
                    model.Magazine.Category = magCat;
                    model.Magazine.Title = String.Format("{0} / {1}", model.Magazine.PublishYear, model.Magazine.PublishSection);
                    model.Magazine.Slug = GenerateSlug(model.Magazine.Title, model.Magazine.PublishYear, model.Magazine.PublishSection);
                    model.Magazine.DateCreated = DateTime.Now;
                }
                else
                {
                    model.Magazine = magazine.FirstOrDefault();
                }

                var author = new Author();
                if (model.Author.Id == 0)
                {
                    author = new Author { Name = model.Author.Name };
                }
                else
                {
                    author = _authorRepo.GetById(model.Author.Id);
                }
                model.Author = author;
                model.PublishedBy = username;

                //foreach (var item in model.ArticleTags)
                //{
                //    var tag = _tagRepo.GetById(Convert.ToInt32(item));
                //}

                _articleRepo.Update(model);
                SaveChanges();
                return new Result<Article>(model);
            }
            catch (Exception ex)
            {
                return new Result<Article>(ResultStatus.ERROR, ex.Message);
            }

        }

        public Result DeleteArticle(int id)
        {
            try
            {
                var article = _articleRepo.GetByIdIncluding(id, x => x.MediaElement);
                foreach (var item in article.MediaElement)
                {
                    _mediaElementRepo.Delete(item);
                }
                _articleRepo.Delete(article);
                SaveChanges();
                return new Result(ResultStatus.OK);
            }
            catch (Exception ex)
            {
                return new Result(ResultStatus.EXCEPTION, ex.Message);
            }
        }
        #endregion
        #endregion

        #region MagazineCategories
        public Result<List<MagazineCategory>> GetAllMagazineCategories()
        {
            try
            {
                var magazineCategories = _magazineCatRepo.GetAllIncluding(x => x.Magazines).ToList();
                if (magazineCategories.Count == 0)
                    return new Result<List<MagazineCategory>>(ResultStatus.NOT_FOUND);

                return new Result<List<MagazineCategory>>(magazineCategories);
            }
            catch (Exception ex)
            {
                return new Result<List<MagazineCategory>>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<MagazineCategory> Add(MagazineCategory model)
        {
            try
            {
                var exists = _magazineCatRepo.FindBy(x => x.Name == model.Name);
                if (exists.Count() > 0)
                    return new Result<MagazineCategory>(ResultStatus.ALREADYEXISTS, "A folyóirat már létezik! Kérem ellenőrizze az adatokat!");

                _magazineCatRepo.Add(model);
                SaveChanges();
                return new Result<MagazineCategory>(model);
            }
            catch (Exception ex)
            {
                return new Result<MagazineCategory>(ResultStatus.ERROR, ex.Message);
            }
        }

        public Result<bool> DeleteMagazineCategory(int id)
        {
            try
            {
                var magazine = _magazineCatRepo.GetByIdIncluding(id, x => x.Magazines);

                if (magazine == null)
                    return new Result<bool>(ResultStatus.NOT_FOUND, "Folyóirat nem található!");

                if (magazine.Magazines != null && magazine.Magazines.Count > 0)
                {
                    foreach (var item in magazine.Magazines)
                    {
                        _magazineRepo.Delete(item);
                    }
                }

                _magazineCatRepo.Delete(magazine);
                SaveChanges();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                return new Result<bool>(ResultStatus.ERROR, ex.Message);
            }
        }


        public Result<MagazineCategory> GetMagazineCategoryById(int id)
        {
            try
            {
                return new Result<MagazineCategory>(_magazineCatRepo.GetById(id));
            }
            catch (Exception ex)
            {
                return new Result<MagazineCategory>(ResultStatus.ERROR, ex.Message);
            }
        }
        #endregion

        #region General
        public string GenerateSlug(string phrase, int year, string section)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            str = string.Format("{0}_{1}_{2}", str, year, section);
            return str;
        }

        public string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }


        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion

        #region Tags
        public Result<List<Tag>> GetAllTags()
        {
            try
            {
                var result = _tagRepo.GetAll();
                if (result == null)
                    return new Result<List<Tag>>(ResultStatus.NOT_FOUND);
                return new Result<List<Tag>>(result.ToList());
            }
            catch (Exception ex)
            {
                return new Result<List<Tag>>(ResultStatus.EXCEPTION, ex.Message);
            }
        }

        public Result<Tag> Add(Tag model)
        {
            try
            {
                model.Name = model.Name.Trim();
                var exists = _tagRepo.FindBy(x => x.Name == model.Name.Trim());
                if (exists.Count() > 0)
                    return new Result<Tag>(ResultStatus.ALREADYEXISTS, "A megadott kulcsszó már létezik!");

                _tagRepo.Add(model);
                SaveChanges();
                return new Result<Tag>(model);
            }
            catch (Exception ex)
            {
                return new Result<Tag>(ResultStatus.EXCEPTION, ex.Message);
            }
        }

        public Result DeleteTag(int id)
        {
            try
            {
                var result = _tagRepo.GetById(id);
                if (result == null)
                    return new Result(ResultStatus.NOT_FOUND);

                _tagRepo.Delete(result);
                SaveChanges();
                return new Result(ResultStatus.OK);
            }
            catch (Exception ex)
            {
                return new Result(ResultStatus.EXCEPTION, ex.Message);
            }
        }

        #endregion

        #region Author
        public Result<List<Author>> GetAllAuthors()
        {
            try
            {
                var result = _authorRepo.GetAll();
                if (result == null)
                    return new Result<List<Author>>(ResultStatus.NOT_FOUND);
                return new Result<List<Author>>(result.ToList());

            }
            catch (Exception e)
            {
                return new Result<List<Author>>(ResultStatus.EXCEPTION, e.Message);
            }
        }
        #endregion

        #region Private
        private void CheckMediaElements(List<Magazine> magazines)
        {
            if (magazines.Count > 0)
            {
                foreach (var magazine in magazines)
                {
                    if (magazine.MediaElements.Count == 0)
                    {
                        var mediaElement = new MediaElement()
                        {
                            Description = "Borito",
                            Name = "Template borito",
                            OriginalName = "images/components/EGY_borito_H.jpg",
                        };
                        magazine.MediaElements.Add(mediaElement);
                    }
                }
            }
        }

        private void CheckMediaElements(Magazine magazine)
        {
            if (magazine != null)
            {
                if (magazine.MediaElements.Count == 0)
                {
                    var mediaElement = new MediaElement()
                    {
                        Description = "Borito",
                        Name = "Template borito",
                        OriginalName = "images/components/template_borito.jpg",
                    };
                    magazine.MediaElements.Add(mediaElement);
                }
            }
        }
        #endregion

        #region XEdit
        public Result<Magazine> Update(XEditSM model)
        {
            var result = _magazineRepo.GetById(model.PrimaryKey);
            if (result == null) return new Result<Magazine>(ResultStatus.NOT_FOUND);

            switch (model.Name)
            {
                case "Visible":
                    var visible = Convert.ToBoolean(model.Value);
                    if (result.Visible == visible) return new Result<Magazine>(result);
                    result.Visible = visible;
                    break;
                case "Title":
                    if (result.Title.Trim() == model.Value.Trim()) return new Result<Magazine>(result);
                    result.Title = model.Value.Trim();
                    break;
                case "Section":
                    if (result.PublishSection.Trim() == model.Value.Trim()) return new Result<Magazine>(result);
                    result.PublishSection = model.Value.Trim();
                    break;
                case "Year":
                    var year = Convert.ToInt32(model.Value);
                    if (result.PublishYear == year) return new Result<Magazine>(result);
                    result.PublishYear = year;
                    break;
                default:
                    return new Result<Magazine>(result);
            }

            _magazineRepo.Update(result);
            SaveChanges();
            return new Result<Magazine>(result);
        }

        public Result<Magazine> UpdateCover(IFormFile files, int id)
        {
            var result = _magazineRepo.GetByIdIncluding(id, x => x.MediaElements);

            if (result == null) return new Result<Magazine>(ResultStatus.NOT_FOUND);

            //var removable = result.MediaElements.FirstOrDefault(x => x.Scope == Data.Entities.Enums.MediaTypesScope.Cover);
            //result.MediaElements.Remove(removable);

            if (files == null || files.Length == 0) return new Result<Magazine>(ResultStatus.ERROR);

            result.MediaElements = CreateMediaElements(files, result.PublishYear, result.PublishSection);

            _magazineRepo.Update(result);
            SaveChanges();
            return new Result<Magazine>(result);
        }

        public Result FormatHtml()
        {
            var articles = _articleRepo.GetAll();
            foreach (var item in articles)
            {
                if (!string.IsNullOrEmpty(item.Content))
                {
                    var content = item.Content;

                    content = content.Replace("\r\n", "<br />");
                    item.Content = content;

                    _articleRepo.Update(item);
                    SaveChanges();
                }
            }
            return new Result(ResultStatus.OK);
        }
        #endregion
    }
}
