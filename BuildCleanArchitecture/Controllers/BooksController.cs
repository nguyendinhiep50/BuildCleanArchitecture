using BuildCleanArchitecture.Application.Books.Commands;
using BuildCleanArchitecture.Application.Books.Dtos;
using BuildCleanArchitecture.Application.Books.Queries;
using BuildCleanArchitecture.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuildCleanArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ApiControllerBase
    {
        [HttpGet]
        public Task<PaginatedList<BookDto>> Get([FromQuery] BookPagingFilterDto filter)
        {
            return Mediator.Send(new GetBooksQuery
            {
                Filter = filter
            });
        }

        [HttpGet("{code}")]
        public Task<BookDto> GetByCode([FromRoute] string code)
        {
            return Mediator.Send(new GetBookByBookCodeQuery
            {
                Id = code
            });
        }

        [HttpPost]
        public Task<ResponseModel<Boolean>> CreateUserAsync([FromBody] BookAddDto writeDto)
        {
            return Mediator.Send(new CreateBookCommand
            {
                Dto = writeDto
            });
        }

        [HttpPut("{code}")]
        public async Task<ResponseModel<bool>> UpdateUserAsync([FromRoute] string code, [FromBody] BookUpdateDto writeDto)
        {
            return await Mediator.Send(new UpdateBookCommand
            {
                Id = code,
                Dto = writeDto
            });
        }

        [HttpDelete("{code}")]
        public Task<ResponseModel<bool>> DeleteUserAsync([FromRoute] string code)
        {
            return Mediator.Send(new DeleteBookCommand
            {
                Id = code
            });
        }
    }
}
