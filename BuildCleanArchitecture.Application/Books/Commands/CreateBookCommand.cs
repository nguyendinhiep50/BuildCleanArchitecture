using AutoMapper;
using BuildCleanArchitecture.Application.Books.Dtos;
using BuildCleanArchitecture.Application.Books.Queries;
using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Common.Models;
using BuildCleanArchitecture.Domain.Entities;
using MediatR;

namespace BuildCleanArchitecture.Application.Books.Commands
{
    public class CreateBookCommand : IRequest<ResponseModel<bool>>
    {
        public BookAddDto Dto { get; set; } = null!;
    }

    internal class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, ResponseModel<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateBookCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ResponseModel<bool>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request.Dto!);

            var existedAccount0Code = await _mediator.Send(new CheckBookCodeExistedQuery
            {
                Id = book.Id!
            });

            if (existedAccount0Code)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = $"Sách này với mã {book.Id} đã tồn tại"
                };
            }

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
    }
}
