using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Swan.Client.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DecryptRawController : Controller
    {
        private readonly ISwanConnection _swanConnection;

        public DecryptRawController(ISwanConnection swanConnection)
        {
            _swanConnection = swanConnection;
        }

        /// <summary>
        /// DecryptRaw returns key value pairs for the raw SWAN data contained 
        /// in the encrypted string. Must only be used by User Interface 
        /// Providers.
        /// </summary>
        /// <param name="url">
        /// Url of the calling page with the encrypted data appended.
        /// </param>
        /// <returns>
        /// A JSON version of the <see cref="Model.Update"/> instance.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("url")]
        [HttpPost("url")]
        public async Task<JsonResult> Get(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                var encrypted = uri.GetEncrypted();
                if (String.IsNullOrEmpty(encrypted) == false)
                {
                    var update = await _swanConnection.DecryptRaw(encrypted);
                    return Json(update);
                }
            }
            return Json(new object[] { });
        }
    }
}
