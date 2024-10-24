using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using protocol.rabbitqm.shared.Interfaces;
using protocol.rabbitqm.shared.Models.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace protocol.rabbitqm.service.Services
{
    public class ServiceAuth : IServiceAuth
    {
        private readonly IConfiguration config;

        public ServiceAuth(IConfiguration config)
        {
            this.config = config;
        }

        public string GerarTokenJWT(string user, int ExpirationValue = 600)
        {
            var userID = user;
            var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userID),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    // Adicione outras reivindicações (roles, permissões etc.) aqui
                };
            var keyString = config["Auth:SecretKey"];
            // Cria a Cryptografia
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = config["Jwt:Issuer"];
            var audience = config["Jwt:Audience"];


            var Newtoken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(ExpirationValue),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(Newtoken);
            // Monta o Token
            return token;
        }


        public string Login(UserRequest sender)
        {
            var usuarios = config.GetSection("Auth").GetSection("User").Get<Dictionary<string, string>>();
            if (usuarios.ContainsKey(sender.User) && usuarios[sender.User] == sender.Pass)
            {
                return GerarTokenJWT(sender.User, 3000);
            }
            return null;
        }

    }
}