using AutoMapper;
using BuildCleanArchitecture.Application.CatalogBooks.Dtos;
using BuildCleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.CatalogBooks.Queries
{
    public class GetCatalogBookByCatalogBookCodeQuery : IRequest<CatalogBookDto>
    {
        public string Id { get; set; } = null!;
    }

    public class GetCatalogBookByCatalogBookCodeQueryHandler : IRequestHandler<GetCatalogBookByCatalogBookCodeQuery, CatalogBookDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCatalogBookByCatalogBookCodeQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CatalogBookDto> Handle(GetCatalogBookByCatalogBookCodeQuery request, CancellationToken cancellationToken)
        {
            var catalogBook = await _context.CatalogBooks.AsNoTracking().Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            return _mapper.Map<CatalogBookDto>(catalogBook);
        }
    }
}
