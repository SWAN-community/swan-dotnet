using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Swan.Client.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UpdateController : Controller
    {
        private readonly ISwanConnection _swanConnection;

        public UpdateController(ISwanConnection swanConnection)
        {
            _swanConnection = swanConnection;
        }

        /// <summary>
        /// Returns the URL to redirect the web browser to.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("url")]
        [HttpPost("url")]
        public async Task<IActionResult> GetUrl(Model.Update update)
        {
            return new ContentResult
            {
                Content = await _swanConnection.Update(
                    HttpContext.Request,
                    update).GetURL(),
                ContentType = "text/plain; charset=utf-8"
            };
        }
    }
}
