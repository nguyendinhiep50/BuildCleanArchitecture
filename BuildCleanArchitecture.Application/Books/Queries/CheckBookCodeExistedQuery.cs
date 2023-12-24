using BuildCleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.Books.Queries
{
    public class CheckBookCodeExistedQuery : IRequest<bool>
    {
        public string Id { get; set; } = null!;
    }

    internal class CheckAccount0CodeExistedQueryHandler : IRequestHandler<CheckBookCodeExistedQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public CheckAccount0CodeExistedQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckBookCodeExistedQuery request, CancellationToken cancellationToken)
        {
            return await _context.Books.AsNoTracking().Where(x => x.Id == request.Id).AnyAsync();
        }
    }
}
