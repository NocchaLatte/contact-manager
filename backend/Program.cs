using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.Sqlite;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsPolicy = "_allowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000") // will be replaced to use the env variable in the future
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// EF core and SQLite setup
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handler middleware
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        var factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        var status = StatusCodes.Status500InternalServerError;
        var title = "Server Error";
        var detail = "An unexpected error occurred.";

        // Handle specific exceptions (e.g., DbUpdateException for unique constraint violations)

        // DbUpdateException -> sqlite error code 19 is unique constraint violation
        if (ex is DbUpdateException dbex &&
        (dbex.InnerException is SqliteException s && s.SqliteErrorCode == 19 ||
        dbex.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true))
        {
            status = StatusCodes.Status409Conflict;
            title = "Conflict";
            detail = "Email already exists.";
        }
        var problem = factory.CreateProblemDetails(
            context,
            statusCode: status,
            title: title,
            detail: detail
        );
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem);
    });
});

app.UseHttpsRedirection();

app.UseCors(corsPolicy);

app.MapControllers();


app.Run();


public partial class Program { } //for testing purposes, to allow the use of the Program class in tests