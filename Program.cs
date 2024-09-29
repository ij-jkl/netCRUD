using Crud_API.Data;
using Crud_API.Repositories.Interfaces;
using Crud_API.Repositories;
using Crud_API.Services.IServices;
using Crud_API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework using environment variable for Data Source
var pcName = Environment.GetEnvironmentVariable("MY_PC_NAME");
var connectionString = $"Data Source={pcName}\\SQLEXPRESS;Initial Catalog=crudAPI;Integrated Security=True;TrustServerCertificate=True;";

// Configure DbContext with the connection string
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();  
builder.Services.AddScoped<IUserService, UserService>();

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
