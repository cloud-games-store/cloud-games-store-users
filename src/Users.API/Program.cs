using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();

#region DI - Services & Repository

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IPasswordHash, PasswordHashService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

#region Database

builder.Services.AddDbContext<UserDbContext>();

#endregion

var app = builder.Build();

#region Apply Migrations

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

using var context = services.GetRequiredService<UserDbContext>();

context.Database.Migrate();

#endregion

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
