using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using protocol.rabbitqm.shared.Helpers;

namespace protocol.rabbitqm.service.DependencyInject
{
    public static partial class DependencyInject
    {
        public static IServiceCollection AddServiceSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Constants.TITLE_APP, Version = "v1" });

                // Configurar a segurança JWT no Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT no formato 'Bearer {seu token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });
            return services;
        }
    }
}
