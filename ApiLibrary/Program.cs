using ApiLibrary.Data;
using ApiLibrary.Helpers;
using ApiLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string cnString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryLibrary>();

//Helper for JwtBearer
HelperActionServiceOAuth helperActionServiceOAuth = new HelperActionServiceOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServiceOAuth>();
builder.Services.AddAuthentication(helperActionServiceOAuth.GetAuthenticationSchema())
    .AddJwtBearer(helperActionServiceOAuth.GetJwtBearerOptions());

builder.Services.AddSingleton<HelperListStatus>();
builder.Services.AddSingleton<HelperCriptography>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
