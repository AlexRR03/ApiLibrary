using ApiLibrary.Data;
using ApiLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string cnString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryLibrary>();
builder.Services.AddDbContext<ProjectGamesContext>(options => options.UseSqlServer(cnString));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/scalar");
        return Task.CompletedTask;
    });
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
