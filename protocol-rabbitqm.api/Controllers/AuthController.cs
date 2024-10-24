using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using protocol.rabbitqm.shared.Interfaces;
using protocol.rabbitqm.shared.Models.Request;

namespace protocol.rabbitqm.api.Controllers
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
            return Ok(response);
        }

    }
}
