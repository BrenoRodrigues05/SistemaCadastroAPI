using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Context;
using SistemaCadastro.Filters;
using SistemaCadastro.Logging;

var builder = WebApplication.CreateBuilder(args);

//  Configura o DbContext
var mysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SistemaCadastroContext>(options =>
    options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));

//  Configura��es b�sicas
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Configura��o de log
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//  Aqui est� a corre��o
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

//  Middlewares padr�o
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
