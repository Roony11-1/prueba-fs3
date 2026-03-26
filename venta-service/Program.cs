using System.IdentityModel.Tokens.Jwt;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using VentaService.Application;
using VentaService.Application.Interfaces;
using VentaService.Domain;
using VentaService.Infrastructure.BackgroundJobs;
using VentaService.Infrastructure.Clients;
using VentaService.Infrastructure.Messaging;
using VentaService.Infrastructure.Middleware;
using VentaService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Di
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IVentaService, VentaService.Application.VentaService>();

builder.Services.AddScoped<IInventarioClient, InventarioClient>();

// Creamos un canal que solo transporta una "señal"
builder.Services.AddSingleton(Channel.CreateUnbounded<bool>());

builder.Services.AddHostedService<OutboxProcessorBackgroundService>();

builder.Services.AddScoped<IEventPublisher>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var bootstrap = config["Kafka:BootstrapServers"] ?? throw new ArgumentException("Kafka:BootstrapServers is not defined");
    return new KafkaProducer(bootstrap);
});

// Registro de la db
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient("InventarioClient", client =>
{
    client.BaseAddress = new Uri("http://inventory-service:5000/");
    client.Timeout = TimeSpan.FromSeconds(5); // No esperar más de 5s por Java
})
.AddStandardResilienceHandler(options =>
{
    // Configurar el Reintento (Retry)
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromSeconds(1);
    options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;

    // Configurar el Cortocircuito (Circuit Breaker)
    options.CircuitBreaker.FailureRatio = 0.5; // Si falla el 50% de las peticiones...
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
    options.CircuitBreaker.MinimumThroughput = 5; // ... de al menos 5 peticiones.
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15); // Abre el circuito por 15s.
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.Authority = "http://localhost:8080/realms/reino-neytan"; // Keycloak
        options.MetadataAddress = "http://keycloak:8080/realms/reino-neytan/.well-known/openid-configuration"; // Donde esta en docker
        options.Audience = "ricardito"; // clientId
        options.RequireHttpsMetadata = false; // nose

        options.TokenValidationParameters = new()
        {
            ValidateAudience = false, // opcional dependiendo config
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:8080/realms/reino-neytan",
            NameClaimType = "sub"
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();

app.UseCors("AllowAll");

// Seguridad
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();
