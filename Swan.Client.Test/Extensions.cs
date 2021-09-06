using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swan.Client.Test
{
    public static class Extensions
    {
        public static readonly Regex _uriRegex = new Regex(
            @"http[s]://[^\""']+",
            RegexOptions.Compiled);
        public static readonly HttpClient _client = new HttpClient(
            new HttpClientHandler()
            {
                AutomaticDecompression =
                    DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

        /// <summary>
        /// A URL that would be passed to the web browser to process needs to 
        /// be called as part of the tests. This extensions method does this
        /// for operations with a single node. It returns the URL that would
        /// be returned to after the SWAN storage operation completes.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<Uri> MockBrowserRedirect(this Uri uri)
        {
            var html = await _client.GetStringAsync(uri);
            var encryptReturnUrl = _uriRegex.Match(html);
            Assert.IsNotNull(encryptReturnUrl);
            Assert.IsTrue(encryptReturnUrl.Success);
            Assert.IsTrue(Uri.TryCreate(
                encryptReturnUrl.Value,
                UriKind.Absolute,
                out uri));
            return uri;
        }
    }
}
