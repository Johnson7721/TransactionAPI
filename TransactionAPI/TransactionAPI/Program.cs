using FluentValidation;
using FluentValidation.AspNetCore;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TransactionAPI.Services;
using TransactionAPI.Helpers.Validator;
using TransactionAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Initialize log4net from log4net.config
// Loads logging configuration (appenders, log levels, file paths) at startup
// so the Logger helper class can write logs throughout the application.
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly()!);
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Disable the default camelCase policy so property names follow
        // the [JsonPropertyName] attributes exactly (e.g., "partnerkey").
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        // Keep dictionary keys as-is (no camelCase conversion).
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
        // Pretty-print JSON responses for readability.
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        // Pick the first non-empty error message, skipping raw JSON
        // deserialization errors (which are not user-friendly).
        var firstError = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault(m => !string.IsNullOrWhiteSpace(m)
                && !m.Contains("JSON", StringComparison.OrdinalIgnoreCase));

        // Wrap the error in the API's standard failure response shape.
        var response = TransactionResponse.Failure(firstError ?? ValidationMessages.InvalidRequestFormat);
        TransactionAPI.Helpers.Logger.Logger.LogWarn(
            $"Validation failed for {context.HttpContext.Request.Path}: {response.ResultMessage}");
        return new BadRequestObjectResult(response);
    };
});

// Enable FluentValidation auto-validation: validators run automatically
// during model binding, so controllers never receive an invalid request.
builder.Services.AddFluentValidationAutoValidation();

// Scan this assembly and register every AbstractValidator<T>
// (e.g., TransactionRequestValidator) in the DI container.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Register application services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IDiscountCalculator, DiscountCalculator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Register services required to generate the OpenAPI (Swagger) document.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Transaction API",
        Version = "v1",
        Description = "API for processing partner transactions with validation and discount calculations"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
