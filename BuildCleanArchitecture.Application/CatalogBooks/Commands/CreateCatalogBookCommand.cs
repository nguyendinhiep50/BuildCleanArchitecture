using AutoMapper;
using BuildCleanArchitecture.Application.CatalogBooks.Dtos;
using BuildCleanArchitecture.Application.CatalogBooks.Queries;
using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Common.Models;
using BuildCleanArchitecture.Domain.Entities;
using MediatR;

namespace BuildCleanArchitecture.Application.CatalogBooks.Commands
{
    public class CreateCatalogBookCommand : IRequest<ResponseModel<bool>>
    {
        public CatalogBookAddDto Dto { get; set; } = null!;
    }

    internal class CreateCatalogBookCommandHandler : IRequestHandler<CreateCatalogBookCommand, ResponseModel<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateCatalogBookCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ResponseModel<bool>> Handle(CreateCatalogBookCommand request, CancellationToken cancellationToken)
        {
            var catalogBook = _mapper.Map<CatalogBook>(request.Dto!);

            var existedCatalogBookCode = await _mediator.Send(new CheckCatalogBookCodeExistedQuery
            {
                Id = catalogBook.Id!
            });

            if (existedCatalogBookCode)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = $"Loại sách này với mã {catalogBook.Id} đã tồn tại"
                };
            }

            await _context.CatalogBooks.AddAsync(catalogBook);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
    }
}
