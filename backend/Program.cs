using backend.Data;
using Microsoft.EntityFrameworkCore;

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

app.UseHttpsRedirection();

app.UseCors(corsPolicy);

app.MapControllers();


app.Run();


public partial class Program { } //for testing purposes, to allow the use of the Program class in tests