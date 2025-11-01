
using System.Text.Json;
using dotnet.Http.Middlewares;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using dotnet.Repositories;
using Microsoft.EntityFrameworkCore;
using dotnet.Common;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SnakeCaseRouteFactory()));
    options.ValueProviderFactories.Insert(0, new SnakeCaseQueryFactory());
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention());
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IServiceRegistration>()
    .AddClasses(classes => classes.AssignableTo<IServiceRegistration>())
    .AsSelfWithInterfaces()
    .WithScopedLifetime());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    Console.WriteLine("Connecting to the database...");
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var canConnect = await db.Database.CanConnectAsync();
    Console.WriteLine(canConnect ? "✅ OK" : "❌ Fail");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiExceptionMiddleware>();
app.UseMiddleware<ValidationMiddleware>();
app.UseMiddleware<RawBodyMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
