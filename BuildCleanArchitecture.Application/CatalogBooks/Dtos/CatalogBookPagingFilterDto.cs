using BuildCleanArchitecture.Application.Common.Models;

namespace BuildCleanArchitecture.Application.CatalogBooks.Dtos
{
    public class CatalogBookPagingFilterDto : PagingFilterModel
    {
        public string? SearchText { get; set; }
    }
}
