using AutoMapper;
using BuildCleanArchitecture.Application.Books.Dtos;
using BuildCleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.Books.Queries
{

    public class GetBookByBookCodeQuery : IRequest<BookDto>
    {
        public string Id { get; set; } = null!;
    }

    public class GetBookByBookCodeQueryHandler : IRequestHandler<GetBookByBookCodeQuery, BookDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBookByBookCodeQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(GetBookByBookCodeQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.AsNoTracking().Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            return _mapper.Map<BookDto>(book);
        }
    }
}
