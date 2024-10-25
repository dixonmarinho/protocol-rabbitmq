using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using protocol.rabbitmq.shared.Interfaces;
using protocol.rabbitmq.shared.Models.Request;

namespace protocol.rabbitmq.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IServiceAuth service;

        public AuthController(IServiceAuth service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserRequest sender)
        {
            var response = service.Login(sender);
            if (response.success == false)
                return Unauthorized("Usuário não autorizado");
            return Ok(response.data);
        }

    }
}
