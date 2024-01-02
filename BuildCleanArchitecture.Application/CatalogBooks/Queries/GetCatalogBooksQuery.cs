using AutoMapper;
using BuildCleanArchitecture.Application.CatalogBooks.Dtos;
using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Common.Models;
using BuildCleanArchitecture.Application.Extensions;
using BuildCleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.CatalogBooks.Queries
{
    public class GetCatalogBooksQuery : IRequest<PaginatedList<CatalogBookDto>>
    {
        public CatalogBookPagingFilterDto Filter { get; set; } = null!;
    }

    public class GetCatalogBooksQueryHandler : IRequestHandler<GetCatalogBooksQuery, PaginatedList<CatalogBookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCatalogBooksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CatalogBookDto>> Handle(GetCatalogBooksQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;
            var query = _context.CatalogBooks.AsNoTracking();

            string searchText = filter.SearchText ?? "";

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => EF.Functions.Like(x.Name!, "%" + searchText + "%")
                                        );
            }

            return await query.ToPagingAsync<CatalogBook, CatalogBookDto>(filter!, _mapper);
        }
    }
}
