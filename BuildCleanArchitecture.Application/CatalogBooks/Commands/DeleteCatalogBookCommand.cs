using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.CatalogBooks.Commands
{
    public class DeleteCatalogBookCommand : IRequest<ResponseModel<bool>>
    {
        public string Id { get; set; } = null!;
    }

    internal class DeleteCatalogBookCommandHandler : IRequestHandler<DeleteCatalogBookCommand, ResponseModel<bool>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCatalogBookCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<bool>> Handle(DeleteCatalogBookCommand request, CancellationToken cancellationToken)
        {
            var catalogBook = await _context.CatalogBooks.AsTracking().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (catalogBook == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = $"Loại sách với mã {request.Id} không tồn tại"
                };
            }

            catalogBook.Status = false;

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
    }
}
