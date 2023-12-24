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

    internal class DeleteAccount0CommandHandler : IRequestHandler<DeleteBookCommand, ResponseModel<bool>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteAccount0CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<bool>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var account0 = await _context.Books.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (account0 == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = $"Sách với mã  {request.Id} không tồn tại"
                };
            }

            account0.Status = false;

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
    }
}
