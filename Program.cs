using Crud_API.Data;
using Crud_API.Repositories.Interfaces;
using Crud_API.Repositories;
using Crud_API.Services.IServices;
using Crud_API.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Crud_API.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Load the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ConnectionDefault");

// Configure DbContext with the connection string
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Register FluentValidation and all validators from the assembly
builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<UserPostDtoValidator>(); // Register all validators in the same assembly as UserPostDtoValidator
});

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
