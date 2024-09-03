using EBA.Forecasting.ServiceDefaults;

using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add Swagger Examples
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.Run();