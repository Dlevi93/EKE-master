﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EKE.Data.Entities;
using EKE.Data.Entities.Gyopar;
using EKE.Data.Entities.Museum;
using System;

namespace EKE.Data
{
    public class BaseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        #region Entities
        DbSet<Article> Articles { get; set; }
        DbSet<Author> Authors { get; set; }
        DbSet<Magazine> Magazines { get; set; }
        DbSet<MagazineCategory> MagazineCategories { get; set; }
        DbSet<MediaElement> MediaElements { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Synonym> Synonyms { get; set; }

        DbSet<Element> M_Element { get; set; }
        DbSet<ElementCategory> M_ElementCategory { get; set; }
        DbSet<ElementTag> M_ElementTag { get; set; }
        #endregion

        //Model configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Magazin to tag relationship
            modelBuilder.Entity<MagazineTag>()
           .HasKey(t => new { t.MagazinId, t.TagId });

            modelBuilder.Entity<MagazineTag>()
                .HasOne(pt => pt.Magazin)
                .WithMany(p => p.MagazineTags)
                .HasForeignKey(pt => pt.MagazinId);

            modelBuilder.Entity<MagazineTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.MagazineTags)
                .HasForeignKey(pt => pt.TagId);

            //Article to tag relationship
            modelBuilder.Entity<ArticleTag>()
            .HasKey(t => new { t.ArticleId, t.TagId });

            modelBuilder.Entity<ArticleTag>()
                .HasOne(pt => pt.Article)
                .WithMany(p => p.ArticleTag)
                .HasForeignKey(pt => pt.ArticleId);

            modelBuilder.Entity<ArticleTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(pt => pt.TagId);
        }

        public virtual void Commit()
        {
            this.SaveChanges();
        }
    }
}
