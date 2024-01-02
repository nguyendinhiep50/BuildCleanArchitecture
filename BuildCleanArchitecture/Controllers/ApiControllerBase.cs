using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuildCleanArchitecture.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private IMediator _mediator = null!;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
    }
}
