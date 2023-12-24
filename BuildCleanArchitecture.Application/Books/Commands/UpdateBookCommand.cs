using AutoMapper;
using BuildCleanArchitecture.Application.Books.Dtos;
using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.Books.Commands
{
    public class UpdateBookCommand : IRequest<ResponseModel<bool>>
    {
        public string Id { get; set; } = null!;
        public BookUpdateDto Dto { get; set; } = null!;
    }

    internal class UpdateAccount0CommandHandler : IRequestHandler<UpdateBookCommand, ResponseModel<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateAccount0CommandHandler(IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel<bool>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id ?? string.Empty;
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = $"Sách với mã {id} không tồn tại"
                };
            }

            _mapper.Map(request.Dto, book);

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
    }
}
