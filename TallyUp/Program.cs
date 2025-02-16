using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TallyUp.Application.Mapping;
using TallyUp.Application.Validators;
using TallyUp.Configurations;
using TallyUp.Domain.Entities;
using TallyUp.Infrastructure.Data;
using TallyUp.Infrastructure.Extensions;
using TallyUp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    // Читаем параметр UseInMemoryDatabase (по умолчанию false)
    var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase", false);

    if (useInMemory)
    {
        options.UseInMemoryDatabase("TestDatabase");
    }
    else
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }
});

builder.Services
    .AddIdentity<ApplicationUser, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.RegisterApplicationServices();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAutoMapper(typeof(PollProfile));

var app = builder.Build();

using var scope = app.Services.CreateScope();
await DbSeeder.SeedAsync(scope.ServiceProvider);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }