using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Users.Application.Interfaces;
using Users.Application.Services;
using Users.Domain.Interfaces;
using Users.Infra.Data.Context;
using Users.Infra.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region DI - Services & Repository

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IPasswordHash, PasswordHashService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

#endregion

#region Database

builder.Services.AddDbContext<UserDbContext>();

#endregion

#region JWT 

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "FIAP Cloud Games - Users", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu token}"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Users.API.xml"));
});

#endregion

#region Health Check

builder.Services.AddHealthChecks().AddSqlServer(
    builder.Configuration.GetConnectionString("DbConnection")!,
    name: "sqlserver",
    failureStatus: HealthStatus.Unhealthy,
    timeout: TimeSpan.FromSeconds(5)
);
#endregion

var app = builder.Build();

#region Health Check Endpoints

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Name == "sqlserver"
});
#endregion

    #region Apply Migrations

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

using var context = services.GetRequiredService<UserDbContext>();

context.Database.Migrate();

#endregion


app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIAP Cloud Games - Users V1");

    c.RoutePrefix = "users/swagger";
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();