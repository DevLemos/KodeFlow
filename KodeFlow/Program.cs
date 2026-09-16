using KodeFlow.Data.Context;
using KodeFlow.Extensions;
using KodeFlow.Filters;
using KodeFlow.Logging;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Evitar serialização cíclica entre os objetos =========================================================================================
builder.Services.AddControllers(options =>
{
    //Adicionando um filtro para exceção global do meu projeto
    options.Filters.Add(typeof(ExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
    ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Busca Connection String e Configura o AppDbContext =========================================================================================
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information); //Loga todas as queries
});

builder.Services.AddScoped<LogExecucaoFilter>();
builder.Services.AddScoped<TempoExecucaoFilter>();

//Adiciona o provedor de log personalizado e define o nível mínimo com LogLevel.Information
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.ConfigureExceptionHandler(); //Utilizando método de extensão
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
