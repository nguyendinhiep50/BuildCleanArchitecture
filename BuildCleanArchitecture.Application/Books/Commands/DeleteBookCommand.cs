using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.Books.Commands
{
    public class DeleteBookCommand : IRequest<ResponseModel<bool>>
    {
        public string Id { get; set; } = null!;
    }

    internal class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, ResponseModel<bool>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteBookCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<bool>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.AsTracking().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (book == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = $"Sách với mã {request.Id} không tồn tại"
                };
            }

            book.Status = false;

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
    }
}
