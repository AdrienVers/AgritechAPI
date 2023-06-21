using agricultureAPI.Data;
using agricultureAPI.Endpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder();

builder.Logging.ClearProviders();
var loggerConfiguration = new LoggerConfiguration().WriteTo.Console();
var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy(name: "CropOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

builder.Services.AddCropServices();

/*
builder.Services.AddDbContext<CropDbContext>(
    opt => opt.UseSqlite("Filename=crops.db"));
*/

builder.Services.AddDbContext<CropDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"));
    }
);


builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CropOrigins");

app.MapGet("/", () => "Hello !");
app.MapGroup("/crops").MapCropEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<CropDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();