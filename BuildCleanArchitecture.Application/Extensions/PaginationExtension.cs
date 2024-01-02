using AutoMapper;
using BuildCleanArchitecture.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.Extensions
{
    public static class PaginationExtension
    {
        public static async Task<PaginatedList<T>> ToPagingAsync<T>(this IQueryable<T> query, PagingFilterModel request)
        {
            var totalItems = await query.CountAsync();
            var data = await query.Skip((request.PageNumber - 1) * request.PageSize)
                                  .Take(request.PageSize)
                                  .ToListAsync();

            return new PaginatedList<T>(data, totalItems, request.PageNumber, request.PageSize);
        }

        public static async Task<PaginatedList<TDto>> ToPagingAsync<T, TDto>(this IQueryable<T> query, PagingFilterModel request, IMapper mapper)
        {
            var totalItems = await query.CountAsync();
            var data = await query.Skip((request.PageNumber - 1) * request.PageSize)
                                  .Take(request.PageSize)
                                  .ToListAsync();

            var dataDto = mapper.Map<List<T>, List<TDto>>(data);

            return new PaginatedList<TDto>(dataDto, totalItems, request.PageNumber, request.PageSize);
        }
    }
}
