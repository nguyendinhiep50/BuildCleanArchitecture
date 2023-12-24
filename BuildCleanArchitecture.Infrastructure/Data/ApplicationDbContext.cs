using Microsoft.EntityFrameworkCore;
using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Domain.Entities;
using BuildCleanArchitecture.Domain.Enities;
using BuildCleanArchitecture.Infrastructure.Configurations;

namespace BuildCleanArchitecture.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<AuthorBook> Authors => Set<AuthorBook>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<CatalogBook> CatalogBooks => Set<CatalogBook>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
