using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Cine.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseInMemoryDatabase("User"));
builder.Services.AddDbContext<SeatContext>(opt =>
    opt.UseInMemoryDatabase("Seat"));
builder.Services.AddDbContext<RowContext>(opt =>
    opt.UseInMemoryDatabase("Row"));
builder.Services.AddDbContext<RoomContext>(opt =>
    opt.UseInMemoryDatabase("Room"));
builder.Services.AddDbContext<MovieContext>(opt =>
    opt.UseInMemoryDatabase("Movie"));
builder.Services.AddDbContext<ReservationContext>(opt =>
    opt.UseInMemoryDatabase("Reservation"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
