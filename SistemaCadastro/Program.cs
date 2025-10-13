using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Context;
using SistemaCadastro.DTOs;
using SistemaCadastro.Filters;
using SistemaCadastro.Interfaces;
using SistemaCadastro.Logging;
using SistemaCadastro.Mappings;
using SistemaCadastro.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configura o DbContext e String de conex�o com MySQL
var mysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SistemaCadastroContext>(options =>
    options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));

// Configura��es b�sicas
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inje��es de depend�ncia
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICadastroRepository, CadastroRepository>();
builder.Services.AddScoped<CadastroMapper>();

// Configura��o de log
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
    LogFilePath = "Logs/sistema-cadastro.log",
    LogToDatabase = true,
    LogToFile = true
});

//builder.Services.AddSingleton<ILoggerProvider, CustomLoggerProvider>();

// Adiciona o filtro global
builder.Services.AddScoped<APILoggingFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<APILoggingFilter>();
}).AddNewtonsoftJson();

var app = builder.Build();

// Middlewares padr�o
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
