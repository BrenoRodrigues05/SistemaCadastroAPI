using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenXmlPowerTools;
using SistemaCadastro.Context;
using SistemaCadastro.DTOs;
using SistemaCadastro.Extensions;
using SistemaCadastro.Filters;
using SistemaCadastro.Interfaces;
using SistemaCadastro.Logging;
using SistemaCadastro.Mappings;
using SistemaCadastro.Models;
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

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira apenas o token JWT (sem 'Bearer')"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Injeções de dependência
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICadastroRepository, CadastroRepository>();
builder.Services.AddScoped<CadastroMapper>();
builder.Services.AddScoped<CadastroService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Autenticação e Autorização com JWT

builder.Services.AddAuthorization();
    
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SistemaCadastroContext>()
    .AddDefaultTokenProviders();

var secretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Chave secreta JWT não configurada.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine($"JWT ERROR: {ctx.Exception.Message}");
            return Task.CompletedTask;
        }
    };

    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
    };       
});

// Politica de aurotização com base em roles

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
});

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

// Middleware customizado para tratamento de erros
app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

// Middlewares padrão
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
