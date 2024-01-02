using BuildCleanArchitecture.Application.Common.Models;

namespace BuildCleanArchitecture.Application.Books.Dtos
{
    public class BookPagingFilterDto : PagingFilterModel
    {
        public string? SearchText { get; set; }
    }
}
