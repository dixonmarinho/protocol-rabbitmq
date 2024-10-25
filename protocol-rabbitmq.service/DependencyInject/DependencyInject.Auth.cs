using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using protocol.rabbitmq.service.Services;
using protocol.rabbitmq.shared.Interfaces;
using System.Text;

namespace protocol.rabbitmq.service.DependencyInject
{
    public static partial class DependencyInject
    {
        public static IServiceCollection AddServiceAuthentication(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>(); // Obter a configuração do serviço
            var secretKey = config["Auth:SecretKey"]; // Obter a key secreta do arquivo de configuração
            // Nao foi informado a chave secreta
            if (secretKey == null)
                return services;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var issuer = new string[] { config["Jwt:Issuer"] };
            var audience = new string[] { config["Jwt:Audience"] };

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; // Em produção, defina como true para exigir HTTPS
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true, // Você pode validar o emissor do token, se necessário
                    ValidateAudience = true, // Você pode validar a audiência do token, se necessário
                    ValidIssuers = issuer,
                    ValidAudiences = audience,
                    IssuerSigningKey = key
                };
                x.RequireHttpsMetadata = false;
            });
            services.AddAuthorization();
            services.AddScoped<IServiceAuth, ServiceAuth>();

            return services;
        }

    }
}
