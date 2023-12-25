using AutoMapper;
using BuildCleanArchitecture.Application.Books.Dtos;
using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Common.Models;
using BuildCleanArchitecture.Application.Extensions;
using BuildCleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.Books.Queries
{
    public class GetBooksQuery : IRequest<PaginatedList<BookDto>>
    {
        public BookPagingFilterDto Filter { get; set; } = null!;
    }

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PaginatedList<BookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBooksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;
            var query = _context.Books.AsNoTracking();

            string searchText = filter.SearchText ?? "";

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => EF.Functions.Like(x.Name!, "%" + searchText + "%")
                                        );
            }

            query = query.Include(x => x.AuthorBooks);

            return await query.ToPagingAsync<Book, BookDto>(filter!, _mapper);
        }
    }
}
