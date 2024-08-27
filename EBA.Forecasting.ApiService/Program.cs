using EBA.Forecasting.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// Define prediction route & handler
app.MapPost("/predict",
    async (EBA_Model.ModelInput input) =>
        await Task.FromResult(EBA_Model.Predict(input)));
app.MapDefaultEndpoints();

app.Run();