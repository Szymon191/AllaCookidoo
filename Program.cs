using AllaCookidoo.Database;
using AllaCookidoo.Middleware;
using AllaCookidoo.Repositories;
using AllaCookidoo.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("AzureConnection");
builder.Services.AddDbContext<AllaCookidoDatabaseContext>(opt => opt.UseSqlServer(connectionString));


//Application Insights
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["ApplicationInsights:InstrumentationKey"]);
builder.Services.AddSingleton<ITelemetryInitializer, OperationCorrelationTelemetryInitializer>();


// Konfiguracja logowania
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Dodaj logowanie do konsoli
builder.Logging.AddDebug();   // Dodaj logowanie do debuggera
builder.Logging.AddApplicationInsights();


var app = builder.Build();

//middleware Correlation ID
app.UseMiddleware<CorelationIdMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
