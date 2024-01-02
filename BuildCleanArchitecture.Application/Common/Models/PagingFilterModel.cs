namespace BuildCleanArchitecture.Application.Common.Models
{
    public class PagingFilterModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
