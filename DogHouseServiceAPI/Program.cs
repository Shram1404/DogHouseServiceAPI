using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Retrieve and validate request limit count from appsettings.json
string requestLimitString = builder.Configuration["RequestLimitCount"] ?? throw new ArgumentException("RequestLimitCount needs to be defined in appsettings.json");
if (!int.TryParse(requestLimitString, out int requestLimit))
    throw new ArgumentException("RequestLimitCount needs to be a valid integer.");

// Retrieve the API version from appsettings.json
string version = (string?)builder.Configuration.GetValue(typeof(string), key: "ApiVersion") ?? throw new ArgumentNullException("ApiVersion is incorrect");

// Add services to the container
builder.Services.AddDbContext<DogHouseServiceAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DogHouseServiceAPIContext") ?? throw new InvalidOperationException("Connection string 'DogHouseServiceAPIContext' not found.")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    if (version == null) throw new ArgumentException(nameof(version));
    c.SwaggerDoc("v1", new OpenApiInfo { Version = version });
});

// Register services with dependency injection
builder.Services.AddScoped<IDogService, DogService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dogs}/{action=Index}/{id?}");

app.UseAuthorization();

// Apply custom middleware for request limiting
app.UseMiddleware<RequestLimitMiddleware>(requestLimit);

app.MapControllers();

app.Run();
