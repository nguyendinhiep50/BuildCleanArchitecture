using BuildCleanArchitecture.Application.CatalogBooks.Commands;
using BuildCleanArchitecture.Application.CatalogBooks.Dtos;
using BuildCleanArchitecture.Application.CatalogBooks.Queries;
using BuildCleanArchitecture.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildCleanArchitecture.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class CatalogBooksController : ApiControllerBase
    {
        [HttpGet]
        public Task<PaginatedList<CatalogBookDto>> Get([FromQuery] CatalogBookPagingFilterDto filter)
        {
            return Mediator.Send(new GetCatalogBooksQuery
            {
                Filter = filter
            });
        }

        [HttpGet("{code}")]
        public Task<CatalogBookDto> GetByCode([FromRoute] string code)
        {
            return Mediator.Send(new GetCatalogBookByCatalogBookCodeQuery
            {
                Id = code
            });
        }

        [HttpPost]
        public Task<ResponseModel<Boolean>> CreateCatalogBookAsync([FromBody] CatalogBookAddDto writeDto)
        {
            return Mediator.Send(new CreateCatalogBookCommand
            {
                Dto = writeDto
            });
        }

        [HttpPut("{code}")]
        public async Task<ResponseModel<bool>> UpdateCatalogBookAsync([FromRoute] string code, [FromBody] CatalogBookUpdateDto writeDto)
        {
            return await Mediator.Send(new UpdateCatalogBookCommand
            {
                Id = code,
                Dto = writeDto
            });
        }

        [HttpDelete("{code}")]
        public Task<ResponseModel<bool>> DeleteCatalogBookAsync([FromRoute] string code)
        {
            return Mediator.Send(new DeleteCatalogBookCommand
            {
                Id = code
            });
        }
    }
}
