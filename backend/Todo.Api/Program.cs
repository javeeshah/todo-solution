using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Repositories;
using Todo.Api.Services;
using Todo.Api.Validators;
using Todo.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Health checks
builder.Services.AddHealthChecks();

// CORS policy to allow requests from local only. Add more for production as needed.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", policy =>
    {
        // Adjust the port as needed for your frontend
        policy
        .WithOrigins(
            "http://localhost:4200", 
            "https://127.0.01:4200"
        )
        .WithMethods("GET", "POST", "PUT", "DELETE")
        .WithHeaders("content-type", "x-requested-wtih");
    });
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(TodoCreateDtoValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Todo.Api.MappingProfiles.TodoMappingProfile));

//Add Entity Framework Core In-Memory Database
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseInMemoryDatabase("TodoList"));

// Register the TodoService for dependency injection
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// register exception handling middleware early so it can catch unhandled exceptions
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowLocal");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Health endpoint
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }
