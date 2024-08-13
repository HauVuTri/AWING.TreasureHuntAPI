
using AWING.TreasureHuntAPI.Interfaces;
using AWING.TreasureHuntAPI.Models;
using AWING.TreasureHuntAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var services = builder.Services;
services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

services.AddDbContext<TreasureHuntDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("TreasureHunt");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSetting:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JwtSetting:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSetting:Key").Value))
    };
});

services.AddControllers().AddJsonOptions(x =>
{
    //This allows the serializer to handle cycles by keeping track of object references.
    //x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});


services.AddTransient<IAuthService, AuthService>();
services.AddScoped<ITreasureMapService, TreasureMapService>();
services.AddScoped<IResultsService, ResultsService>();


var app = builder.Build();
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
