using BuildCleanArchitecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace BuildCleanArchitecture.Middlewares
{
    public class MiddleWare
    {
        private readonly RequestDelegate _next;

        public MiddleWare(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            // Retrieve the controller and action information
            var endpoint = context.GetEndpoint();
            var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            // Check if the action or controller has [Authorize] attribute
            bool requiresAuthorization = HasAuthorizeAttribute(controllerActionDescriptor);
            if (!requiresAuthorization)
            {
                await _next(context);
                return;
            }

            var currentUser = context.RequestServices.GetService<IUser>()!;

            await _next(context);
            return;
        }

        private bool HasAuthorizeAttribute(ControllerActionDescriptor controllerActionDescriptor)
        {
            if (controllerActionDescriptor == null)
            {
                return false;
            }

            // Check if the action or controller has [Authorize] attribute
            var authorizeAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
                .Union(controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>()
                .FirstOrDefault();

            var anonymousAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
               .Union(controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(true))
               .OfType<AllowAnonymousAttribute>()
               .FirstOrDefault();

            return anonymousAttribute == null && authorizeAttribute != null;
        }
    }

    public static class MiddleWareExtensions
    {
        public static IApplicationBuilder UseUserAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddleWare>();
        }
    }
}
