using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Context;
using SistemaCadastro.Filters;
using SistemaCadastro.Logging;

var builder = WebApplication.CreateBuilder(args);

//  Configura o DbContext
var mysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SistemaCadastroContext>(options =>
    options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));

//  Configurações básicas
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Configuração de log
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//  Aqui está a correção
builder.Services.AddSingleton<CustomLoggerProviderConfiguration>(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
    LogFilePath = "Logs/sistema-cadastro.log",
    LogToDatabase = true,
    LogToFile = true
});

builder.Services.AddSingleton<ILoggerProvider, CustomLoggerProvider>();

//  Adiciona o filtro global
builder.Services.AddScoped<APILoggingFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<APILoggingFilter>();
});

var app = builder.Build();

//  Middlewares padrão
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
