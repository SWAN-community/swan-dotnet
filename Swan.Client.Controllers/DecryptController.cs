using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Swan.Client.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DecryptController : Controller
    {
        private readonly ISwanConnection _swanConnection;

        public DecryptController(ISwanConnection swanConnection)
        {
            _swanConnection = swanConnection;
        }

        /// <summary>
        /// Decrypt returns SWAN key value pairs for the data contained in the
        /// encrypted string.
        /// </summary>
        /// <param name="url">
        /// Url of the calling page with the encrypted data appended.
        /// </param>
        /// <returns>
        /// SWAN key and value pairs where the value is an OWID string.
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
                    var pairs = await _swanConnection.Decrypt(encrypted);
                    return Json(pairs);
                }
            }
            return Json(new object[] { });
        }
    }
}
