using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using API.Services; // Adjust namespaces according to your project structure
using Data;
using Data.Repository;
using System;
using Data.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1",new OpenApiInfo { Title="API",Version="v1"});
});

builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBCS")));
// Register repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
// Register services
builder.Services.AddScoped<IAuthorService, AuthorService>(); // Example, add other services similarly
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IBookCategoryService,BookCategoryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
