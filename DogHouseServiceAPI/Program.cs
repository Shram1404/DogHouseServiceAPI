using Microsoft.EntityFrameworkCore;
using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DogHouseServiceAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DogHouseServiceAPIContext") ?? throw new InvalidOperationException("Connection string 'DogHouseServiceAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

app.MapControllers();

app.Run();

//Пофіксити назву контролера в запиті
//Додати затримку
//Написати юніти тести
//Виправити можливі помилки в валідації
//Додати версію
//Додати коментарі