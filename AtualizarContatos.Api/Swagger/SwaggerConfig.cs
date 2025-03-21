using AtualizarContatos.Domain.Requests;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace AtualizarContatos.Api.Swagger
{
    /// <summary>
    /// Configurações do Swagger para a API de Cadastro de Contatos.
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Adiciona a configuração do Swagger aos serviços.
        /// </summary>
        /// <param name="services">A coleção de serviços da aplicação.</param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cadastro de Contatos - Fase 3",
                    Version = "v1",
                    Description = "Uma API para cadastrar contatos"
                });

                // Adicione exemplos diretamente na configuração do Swagger
                c.MapType<ContatoRequest>(() => new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["id"] = new OpenApiSchema
                        {
                            Type = "int",
                            Example = new OpenApiString("0")
                        },
                        ["nome"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("João Silva")
                        },
                        ["telefone"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("123456789")
                        },
                        ["email"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("joao.silva@example.com")
                        },
                        ["ddd"] = new OpenApiSchema
                        {
                            Type = "integer",
                            Example = new OpenApiInteger(11)
                        }
                    }
                });

                // Obter o caminho do arquivo XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Configura o middleware do Swagger na aplicação.
        /// </summary>
        /// <param name="app">O construtor da aplicação.</param>
        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1"));
        }
    }
}
