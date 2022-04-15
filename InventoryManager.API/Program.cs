using InventoryManager.API.Data;
using InventoryManager.API.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
// Setup services
builder.Services.AddSqlite<InventoryDb>("Data Source=Data/Inventory.db; Cache=Shared;");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddCors();
builder.Logging.AddJsonConsole();

var app = builder.Build();
await EnsureDb(app.Services, app.Logger);
app.MapSwaggerEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader()
      .AllowAnyMethod()
      .AllowAnyOrigin());

app.MapProductEndPoints();
app.MapErrorEndPoints();



app.Run();

async Task EnsureDb(IServiceProvider services, ILogger logger)
{
    await using var db = services.CreateScope().ServiceProvider.GetRequiredService<InventoryDb>();
    if (db.Database.IsRelational())
    {
        logger.LogInformation("Updating database...");
        await db.Database.MigrateAsync();
        logger.LogInformation("Updated database");
    }
}

public partial class Program { }





