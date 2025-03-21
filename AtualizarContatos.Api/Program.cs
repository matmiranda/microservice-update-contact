using AtualizarContatos.Api.Swagger;
using AtualizarContatos.Infrastructure.Exceptions;
using AtualizarContatos.Service.Contato;
using Prometheus;
using AtualizarContatos.Infrastructure.MassTransit;
using AtualizarContatos.Service.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de health check
builder.Services.AddHealthChecks();

// Adiciona a configuração do appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // Permite sobrescrever com variáveis de ambiente

// Configurar MassTransit
builder.Services.ConfigureMassTransit(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("NamedClient", client =>
{
    client.BaseAddress = new Uri("https://fiap-api-gateway.azure-api.net/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddScoped<IContatoService, ContatoService>();

builder.Services.AddScoped<IRabbitMqPublisherService, RabbitMqPublisherService>();

var app = builder.Build();

// Mapeia o endpoint de health check
app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adicionar middleware do Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
