using Microsoft.AspNetCore.Http;
using Swan.Client.Model;
using System.Collections.Generic;

namespace Swan.Client
{
    public interface IConnection
    {
        /// <summary>
        /// The HTTP or HTTPS scheme to use for SWAN requests
        /// </summary>
        string Scheme { get; }

        /// <summary>
        /// Domain name of the SWAN Operator access node
        /// </summary>
        string AccessNode { get; }

        /// <summary>
        /// SWAN access key provided by the SWAN Operator
        /// </summary>
        string AccessKey { get; }

        /// <summary>
        /// NewFetch creates a new fetch operation using the default in the 
        /// connection.
        /// </summary>
        /// <param name="request">http request from a web browser</param>
        /// <param name="returnUrl">
        /// return URL after the operation completes
        /// </param>
        /// <param name="existing">
        /// if any values already exist then use these if none are available in
        /// SWAN
        /// </param>
        /// <returns></returns>
        Fetch Fetch(
            HttpRequest request, 
            string returnUrl, 
            IEnumerable<Pair> existing);

        /// <summary>
        /// NewUpdate creates a new fetch operation using the default in the 
        /// connection.
        /// </summary>
        /// <param name="request">http request from a web browser</param>
        /// <param name="returnUrl">
        /// return URL after the operation completes
        /// </param>
        /// <returns></returns>
        Update NewUpdate(HttpRequest request, string returnUrl);

        /// <summary>
        /// NewStop creates a new stop operation using the default in the 
        /// connection.
        /// </summary>
        /// <param name="request">http request from a web browser</param>
        /// <param name="returnUrl">
        /// return URL after the operation completes
        /// </param>
        /// <param name="host">associated with the advert to stop</param>
        /// <returns></returns>
        Stop NewStop(HttpRequest request, string returnUrl, string host);

        // request http request from a web browser
        /// <summary>
        /// NewClient creates a new request.
        /// </summary>
        /// <param name="request">http request from a web browser</param>
        /// <returns></returns>
        Model.Client NewClient(HttpRequest request);

        /// <summary>
        /// NewDecrypt creates a new decrypt request using the default in the
        /// connection.
        /// </summary>
        /// <param name="encrypted">
        /// the base 64 encoded SWAN data to be decrypted
        /// </param>
        /// <returns></returns>
        Decrypt NewDecrypt(string encrypted);
    }
}
