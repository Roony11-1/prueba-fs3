using Microsoft.EntityFrameworkCore;
using VentaService.Application;
using VentaService.Domain;
using VentaService.Infrastructure.Messaging;
using VentaService.Infrastructure.Middleware;
using VentaService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Di
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IVentaService, VentaService.Application.VentaService>();

builder.Services.AddScoped<IEventPublisher>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var bootstrap = config["Kafka:BootstrapServers"] ?? throw new ArgumentException("Kafka:BootstrapServers is not defined");
    return new KafkaProducer(bootstrap);
});

// Registro de la db
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Polly para manejar reintentos o fallos
//builder.Services.AddHttpClient("VentaServiceClient")
//    .AddPolicyHandler(Policy.Handle<HttpRequestException>()
//        .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)))); // Ejemplo de política de reintento exponencial

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();
