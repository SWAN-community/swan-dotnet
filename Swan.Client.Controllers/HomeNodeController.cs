using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Swan.Client.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HomeNodeController : Controller
    {
        private readonly ISwanConnection _swanConnection;

        public HomeNodeController(ISwanConnection swanConnection)
        {
            _swanConnection = swanConnection;
        }

        /// <summary>
        /// Returns the URL to redirect the web browser to.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet()]
        [HttpPost()]
        public async Task<IActionResult> Get()
        {
            return new ContentResult
            {
                Content = await _swanConnection.NewClient(
                    HttpContext.Request).HomeNode(),
                ContentType = "text/plain; charset=utf-8"
            };
        }
    }
}
