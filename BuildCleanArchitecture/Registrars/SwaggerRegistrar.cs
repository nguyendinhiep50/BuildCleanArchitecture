using Microsoft.OpenApi.Models;

namespace BuildCleanArchitecture.Registrars
{
    public class SwaggerRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerGeneratorOptions.SwaggerDocs.Add("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "MY API version 1",
                    Description = "This is my API in version1",
                });
                setupAction.SwaggerGeneratorOptions.SwaggerDocs.Add("v2", new OpenApiInfo()
                {
                    Version = "v2",
                    Title = "MY API version 2",
                    Description = "This is my API in version2",
                });
            });
        }
    }
}
