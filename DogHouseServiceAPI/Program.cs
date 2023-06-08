using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string requestLimitString = builder.Configuration["RequestLimitCount"] ?? throw new ArgumentException("RequestLimitCount needs to be defined in appsettings.json");
if (!int.TryParse(requestLimitString, out int requestLimit))
    throw new ArgumentException("RequestLimitCount needs to be a valid integer.");

string version = (string?)builder.Configuration.GetValue(typeof(string), key: "ApiVersion") ?? throw new ArgumentNullException("ApiVersion is incorrect");

builder.Services.AddDbContext<DogHouseServiceAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DogHouseServiceAPIContext") ?? throw new InvalidOperationException("Connection string 'DogHouseServiceAPIContext' not found.")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    if (version == null) throw new ArgumentException(nameof(version));
    c.SwaggerDoc("v1", new OpenApiInfo { Version = version });
});

builder.Services.AddScoped<IDogService, DogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.UseMiddleware<RequestLimitMiddleware>(requestLimit);
app.MapControllers();

app.Run();
