using BuildCleanArchitecture.Domain.Enities;
using BuildCleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<AuthorBook> Authors {get;}
        DbSet<Book> Books { get; }
        DbSet<CatalogBook> CatalogBooks { get; }
    }
}
