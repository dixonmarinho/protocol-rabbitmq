using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using protocol.rabbitmq.shared;
using protocol.rabbitmq.shared.Interfaces;
using protocol.rabbitmq.shared.Models.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace protocol.rabbitmq.service.Services
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
                };
            var keyString = config["Auth:SecretKey"];
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
            return $"Bearer {token}";
        }

        public Result<string> Login(UserRequest sender)

        {
            var users = config.GetSection("Auth").GetSection("User").Get<Dictionary<string, string>>();
            if (users.ContainsKey(sender.User) && users[sender.User] == sender.Pass)
                return Result<string>.SuccessData(GerarTokenJWT(sender.User, 3000));
            else
                return Result<string>.Fail();
        }

    }
}