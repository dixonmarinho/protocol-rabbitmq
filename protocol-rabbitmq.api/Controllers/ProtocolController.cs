using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using protocol.rabbitmq.shared.Interfaces;

namespace protocol.rabbitmq.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProtocolController : Controller
    {
        private readonly IServiceProtocol service;

        public ProtocolController(IServiceProtocol service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("{document}")]
        public async Task<IActionResult> Get(string document)
        {
            var response = await service.SearchProtocolsAsync(document);
            if (response.success == false)
                return NotFound(response.xmessage);
            return Ok(response.data);
        }
    }
}
