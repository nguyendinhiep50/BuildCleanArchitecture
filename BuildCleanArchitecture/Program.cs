using BuildCleanArchitecture;
using BuildCleanArchitecture.Application;
using BuildCleanArchitecture.Infrastructure;
using BuildCleanArchitecture.Infrastructure.Data;
using BuildCleanArchitecture.Infrastructure.Identity;
using BuildCleanArchitecture.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Đây là cấu hình để Swagger không yêu cầu xác thực
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt 

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true; // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

});

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nguyendinhiep_key_longdaithonglong"));

builder.Services.AddAuthentication(options =>
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Management");
        policy.RequireRole("Teacher");
        policy.RequireRole("Student");
    });
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
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
