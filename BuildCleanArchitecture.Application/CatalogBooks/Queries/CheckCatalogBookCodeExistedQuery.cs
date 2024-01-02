using BuildCleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.CatalogBooks.Queries
{
    public class CheckCatalogBookCodeExistedQuery : IRequest<bool>
    {
        public string Id { get; set; } = null!;
    }

    internal class CheckCatalogBookCodeExistedQueryHandler : IRequestHandler<CheckCatalogBookCodeExistedQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public CheckCatalogBookCodeExistedQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckCatalogBookCodeExistedQuery request, CancellationToken cancellationToken)
        {
            return await _context.CatalogBooks.AsNoTracking().Where(x => x.Id == request.Id).AnyAsync();
        }
    }
}
