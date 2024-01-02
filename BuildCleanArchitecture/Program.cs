using BuildCleanArchitecture;
using BuildCleanArchitecture.Application;
using BuildCleanArchitecture.Extensions;
using BuildCleanArchitecture.Infrastructure;
using BuildCleanArchitecture.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices(typeof(Program));

var services = builder.Services;
var configuration = builder.Configuration;

services.AddApplicationServices();
services.AddInfrastructureServices(configuration);
services.AddWebServices();

services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
    builder => builder
        .SetIsOriginAllowed((origin) => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});
// Add services to the container.

builder.Services.AddControllers();
services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nguyendinhiep_key_longdaithonglong"));

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-issuer",
            ValidAudience = "your-audience",
            IssuerSigningKey = key
        };
    });

var app = builder.Build();

app.UseCors("AllowMyOrigin");

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.UseUserAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");
});
app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
 