﻿using EKE.Data.DataViewModels;
using EKE.Data.Entities.Gyopar;
using EKE.Data.Infrastructure;
using EKE.Data.Repository;
using EKE.Service.Utils;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EKE.Service.Services.Admin
{
    public interface IArticleService : IBaseService
    {
        Result<List<Article>> Get(ArticleSearch model);
        Result<int> Count(ArticleSearch filter);
    }

    public class ArticleService : BaseService, IArticleService
    {
        private readonly IEntityBaseRepository<Article> _articleRepo;
        public ArticleService(IEntityBaseRepository<Article> articleRepo, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _articleRepo = articleRepo;
        }

        public Result<List<Article>> Get(ArticleSearch filter)
        {
            try
            {
                var predicate = GetSearchPredicate(filter);
                var result = _articleRepo.FindByIncluding(predicate, GetSortedFunction(filter), filter.OrderDirection, filter.Page, filter.Display, p => p.Magazine, p => p.Author).ToList();
                return new Result<List<Article>>(result);
            }
            catch (Exception ex)
            {
                return new Result<List<Article>>(ex);
            }
        }

        public Result<int> Count(ArticleSearch filter)
        {
            try
            {
                var predicate = GetSearchPredicate(filter);
                var result = _articleRepo.Count(predicate);
                return new Result<int>(result);
            }
            catch (Exception ex)
            {
                return new Result<int>(ex);
            }
        }

        private static Func<Article, object> GetSortedFunction(ArticleSearch filter)
        {
            if (filter == null) return new Func<Article, object>(q => q.DateCreated);
            switch (filter.Order)
            {
                case 1:
                    return new Func<Article, object>(c => c.Title);
                default:
                    return new Func<Article, object>(q => q.DateCreated);
            }
        }

        private Expression<Func<Article, bool>> GetSearchPredicate(ArticleSearch filter)
        {
            var predicate = PredicateBuilder.New<Article>(true);
            if (filter.ClearKeyword != null)
            {
                predicate = predicate.And(p => p.ArticleTag.Any(q => q.Tag.Name.Contains(filter.Keyword)) || p.Author.Name.ToLower().Contains(filter.Keyword) ||
                 p.Content.ToLower().Contains(filter.Keyword) || p.Title.ToLower().Contains(filter.Keyword));
            }
            if (filter.PublishYear != 0)
                predicate = predicate.And(p => p.Magazine != null && p.Magazine.PublishYear == filter.PublishYear);
            if (!string.IsNullOrEmpty(filter.PublishSection) && filter.PublishSection != "0")
                predicate = predicate.And(p => p.Magazine != null && p.Magazine.PublishSection.Contains(filter.PublishSection));
            return predicate;
        }
    }
}
