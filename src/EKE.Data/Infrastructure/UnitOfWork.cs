using System;
using EKE.Data.Repository.General;
using EKE.Data.Repository.Gyopar;
using EKE.Data.Repository.Muzeum;
using EKE.Data.Repository.Vt;

namespace EKE.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BaseDbContext _dbContext;
        private IArticleRepository _articleRepository;

        private IMagazineCategoryRepository _magazineCategoryRepository;
        private IMagazineRepository _magazineRepository;
        private IAuthorRepository _authorRepository;
        private IMediaElementRepository _mediaElementRepository;
        private ISynonymRepository _synonymRepository;
        private IOrderRepository _orderRepository;
        private ITagRepository _tagRepository;

        private IElementRepository _elementRepository;
        private IElementCategoryRepository _elementCategoryRepository;
        private IElementTagsRepository _elementTagRepository;

        private IVtAccomodationTypeRepository _accomodationTypeRepository;
        private IVtMembershipRepository _membershipRepository;
        private IVtTripRepository _tripRepository;

        public UnitOfWork(BaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IArticleRepository ArticleRepository => _articleRepository ?? (_articleRepository = new ArticleRepository(_dbContext));
        public IMagazineCategoryRepository MagazineCategoryRepository => _magazineCategoryRepository ?? (_magazineCategoryRepository = new MagazineCategoryRepository(_dbContext));
        public IMagazineRepository MagazineRepository => _magazineRepository ?? (_magazineRepository = new MagazineRepository(_dbContext));
        public IAuthorRepository AuthorRepository => _authorRepository ?? (_authorRepository = new AuthorRepository(_dbContext));
        public IMediaElementRepository MediaElementRepository => _mediaElementRepository ?? (_mediaElementRepository = new MediaElementRepository(_dbContext));
        public ISynonymRepository SynonymRepository => _synonymRepository ?? (_synonymRepository = new SynonymRepository(_dbContext));
        public IOrderRepository OrderRepository => _orderRepository ?? (_orderRepository = new OrderRepository(_dbContext));
        public ITagRepository TagRepository => _tagRepository ?? (_tagRepository = new TagRepository(_dbContext));
        public IElementRepository ElementRepository => _elementRepository ?? (_elementRepository = new ElementRepository(_dbContext));
        public IElementCategoryRepository ElementCategoryRepository => _elementCategoryRepository ?? (_elementCategoryRepository = new ElementCategoryRepository(_dbContext));
        public IElementTagsRepository ElementTagRepository => _elementTagRepository ?? (_elementTagRepository = new ElementTagRepository(_dbContext));
        public IVtAccomodationTypeRepository AccomodationTypeRepository => _accomodationTypeRepository ?? (_accomodationTypeRepository = new VtAccomodationTypeRepository(_dbContext));
        public IVtMembershipRepository MembershipRepository => _membershipRepository ?? (_membershipRepository = new VtMembershipRepository(_dbContext));
        public IVtTripRepository TripRepository => _tripRepository ?? (_tripRepository = new VtTripRepository(_dbContext));

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        #region IDisposable
        ~UnitOfWork()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext?.Dispose();
            }
        }
        #endregion
    }
}
