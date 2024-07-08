using BaseLib.DTOs;
using Microsoft.EntityFrameworkCore;
using ServerLib.Data;
using ServerLib.Helpers;
using ServerLib.Implementations;
using ServerLib.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Services
builder.Services.AddDbContext<BlazexDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlazexDbContext") ??
        throw new InvalidOperationException("Connection string 'BlazexDbContext' not found."));
});
builder.Services.Configure<JwtHelper>(builder.Configuration.GetSection("JwtSections"));
builder.Services.AddScoped<IUserAccountActions, UserAccountActions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
