using System.Text;
using System.Text.Json;
using dotnet.Http.Middlewares;
using dotnet.Repositories;
using dotnet.Common;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// ===== CORS =====
builder.Services.AddCors(options =>
    options.AddPolicy("AllowClient", policy =>
        policy.WithOrigins(config["Client:BaseUrl"]!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()));

// ===== Controllers =====
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SnakeCaseRouteFactory()));
    options.ValueProviderFactories.Insert(0, new SnakeCaseQueryFactory());
    options.Filters.Add<OnlyActionsFilter>();
})
.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy =
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>("Database");
builder.Services.AddHttpClient();

// ===== Database =====
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
    options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

// ===== Dependency Injection =====
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IServiceRegistration>()
    .AddClasses(classes => classes.AssignableTo<IServiceRegistration>())
    .AsSelfWithInterfaces()
    .WithScopedLifetime());

// ===== JWT Authentication =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
    });

var app = builder.Build();

// ===== Middlewares =====
app.UseCors("AllowClient");
app.UseRouting();
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseMiddleware<ValidationMiddleware>();
app.UseMiddleware<RawBodyMiddleware>();
app.UseAuthentication();
app.UseMiddleware<CurrentUserMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();