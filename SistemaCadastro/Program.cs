using DocumentFormat.OpenXml;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using SistemaCadastro.Context;
using SistemaCadastro.DTOs;
using SistemaCadastro.Filters;
using SistemaCadastro.Interfaces;
using SistemaCadastro.Logging;
using SistemaCadastro.Mappings;
using SistemaCadastro.Repositories;
using SistemaCadastro.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configura o DbContext e String de conexão com MySQL
var mysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SistemaCadastroContext>(options =>
    options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));

// Configurações básicas
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger para documentação da API

builder.Services.AddSwaggerGen(c => { 
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Sistema de Cadastro API",
        Description = "API para gerenciamento de cadastros de usuários.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Suporte API",
            Url = new Uri("https://example.com/support")
        }

    });
    // Caminho do arquivo XML gerado pela documentação
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // Inclui os comentários XML no Swagger
    c.IncludeXmlComments(xmlPath);
    });

// Injeções de dependência
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICadastroRepository, CadastroRepository>();
builder.Services.AddScoped<CadastroMapper>();
builder.Services.AddScoped<CadastroService>();

// Configuração de log
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

// Middlewares padrão
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
