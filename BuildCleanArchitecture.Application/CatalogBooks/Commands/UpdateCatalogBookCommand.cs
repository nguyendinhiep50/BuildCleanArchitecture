using AutoMapper;
using BuildCleanArchitecture.Application.CatalogBooks.Dtos;
using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.CatalogBooks.Commands
{
    public class UpdateCatalogBookCommand : IRequest<ResponseModel<bool>>
    {
        public string Id { get; set; } = null!;
        public CatalogBookUpdateDto Dto { get; set; } = null!;
    }

    internal class UpdateCatalogBookCommandHandler : IRequestHandler<UpdateCatalogBookCommand, ResponseModel<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCatalogBookCommandHandler(IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel<bool>> Handle(UpdateCatalogBookCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id ?? string.Empty;
            var catalogBook = await _context.CatalogBooks.AsTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (catalogBook == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = $"Loại sách với mã {id} không tồn tại"
                };
            }

            _mapper.Map(request.Dto, catalogBook);

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
    }
}
