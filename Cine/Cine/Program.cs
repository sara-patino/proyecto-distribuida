using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Cine.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();

// Configuración para MovieContext
builder.Services.AddDbContext<MovieContext>(opt =>
    opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración para RoomContext
builder.Services.AddDbContext<RoomsContext>(opt =>
    opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración para Reservation
builder.Services.AddDbContext<ReservationContext>(opt =>
    opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
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

// Usar CORS antes de otros middlewares
app.UseCors();

// Map both controllers
app.MapControllers();

app.Run();
