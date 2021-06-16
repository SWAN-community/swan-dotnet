using System.Text.Json;
using Swan.Client.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Swan.Client
{
    public static class Extensions
    {
        /// <summary>
        /// GetURL contacts the SWAN operator domain with the access key and 
        /// returns a URL string that the web browser should be immediately 
        /// directed to.
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetURL(this Fetch fetch)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            fetch.SetData(parameters);
            return await fetch.RequestAsString("fetch", parameters);
        }

        /// <summary>
        /// GetURL contacts the SWAN operator domain with the access key and 
        /// returns a URL string that the web browser should be directed to.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static async Task<string> GetURL(this Update update)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            update.SetData(parameters);
            return await update.RequestAsString("update", parameters);
        }

        public static async Task<Pair[]> Decrypt(this Decrypt decrypt)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            decrypt.SetData(parameters);
            var stream = await decrypt.RequestAsStream(
                "decrypt",
                parameters);
            return await JsonSerializer.DeserializeAsync<Pair[]>(stream);
        }

        public static async Task<Update> 
            DecryptRaw(this Decrypt decrypt)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            decrypt.SetData(parameters);
            var stream = await decrypt.RequestAsStream(
                "decrypt-raw",
                parameters);
            return await JsonSerializer.DeserializeAsync<Update>(stream);
        }

        public static string GetEncrypted(this Uri uri, string returnUrl)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            if (returnUrl == null)
            {
                throw new ArgumentNullException("returnUrl");
            }
            if (uri.OriginalString.StartsWith(returnUrl) == false)
            {
                throw new ArgumentException(
                    "Uri must start with returnUrl", 
                    "uri");
            }
            return uri.OriginalString.Substring(returnUrl.Length);
        }
    }
}
