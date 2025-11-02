
using System.Text.Json;
using dotnet.Http.Middlewares;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using dotnet.Repositories;
using Microsoft.EntityFrameworkCore;
using dotnet.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using dotnet.Http.Responses;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SnakeCaseRouteFactory()));
    options.ValueProviderFactories.Insert(0, new SnakeCaseQueryFactory());
    options.Filters.Add<OnlyActionsFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>("Database");
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention());
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IServiceRegistration>()
    .AddClasses(classes => classes.AssignableTo<IServiceRegistration>())
    .AsSelfWithInterfaces()
    .WithScopedLifetime());
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Events.OnRedirectToLogin = async context => ApiResponse.Fail("Unauthorized", status: 401);
    options.Events.OnRedirectToAccessDenied = async context => ApiResponse.Fail("Access denied", status: 403);
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.CallbackPath = "/signin-google";

    options.Scope.Add("email");
    options.Scope.Add("profile");

    options.ClaimActions.MapJsonKey("email", "email");
    options.ClaimActions.MapJsonKey("name", "name");
    options.ClaimActions.MapJsonKey("given_name", "given_name");
    options.ClaimActions.MapJsonKey("sub", "sub");
    options.ClaimActions.MapJsonKey("picture", "picture", "url");

    options.SaveTokens = true;
    options.Events.OnCreatingTicket = context => Task.CompletedTask;
});
var app = builder.Build();

app.UseHttpsRedirection();
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
