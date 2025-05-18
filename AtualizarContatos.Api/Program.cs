using AtualizarContatos.Api.Swagger;
using AtualizarContatos.Infrastructure.Exceptions;
using AtualizarContatos.Service.Contato;
using Prometheus;
using AtualizarContatos.Infrastructure.MassTransit;
using AtualizarContatos.Service.RabbitMq;
using Serilog;

// grava logs em um arquivo no kubernete k8s azure
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("/app/logs/producer/atualizar-contatos/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Adiciona o servi�o de health check
builder.Services.AddHealthChecks();

// Adiciona a configura��o do appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // Permite sobrescrever com vari�veis de ambiente

// Configurar MassTransit
builder.Services.ConfigureMassTransit(builder.Configuration);

// Monitoramento com Application Insights
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

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
app.MapHealthChecks("/atualizar/contato/health");

app.UseSwagger(c =>
{
    c.RouteTemplate = "atualizar/contato/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/atualizar/contato/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "atualizar/contato/swagger";
});

// Adicionar middleware do Prometheus com endpoint customizado
app.UseMetricServer("/atualizar/contato/metrics");

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
