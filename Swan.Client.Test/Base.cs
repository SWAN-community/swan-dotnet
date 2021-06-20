using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owid.Client;
using Swan.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swan.Client.Test
{
    public abstract class Base
    {
        protected const string Scheme = "https";
        protected const string AccessNode = "51db.uk";
        protected const string BadAccessNode = "51dz.uk";
        protected const string OwidDomain = "test.org";
        protected const string StopDomain = "test.org";
        protected const string AccessKey = "CMPKeySWAN";
        protected const string BadAccessKey = "_CMPKeySWAN";
        protected const string ReturnUrl = "https://test.com/index.html?";
        protected const string BadReturnUrl = "http*://test.com/index.html?";
        protected const string Payload = "Hello World";
        protected const string Email = "test@localhost.com";
        protected static readonly Operation _operation = new Operation()
        {
            BackgroundColor = "white",
            DisplayUserInterface = true,
            JavaScript = false,
            Message = "Testing is essential",
            MessageColor = "black",
            NodeCount = 1,
            PostMessageOnComplete = false,
            ProgressColor = "blue",
            Title = "Testing",
            UseHomeNode = true
        };

        protected static readonly Regex _uriRegex = new Regex(
            @"http[s]://[^\""']+",
            RegexOptions.Compiled);
        protected static readonly HttpContext _httpContext =
            new DefaultHttpContext();
        protected static readonly HttpClient _client = new HttpClient(
            new HttpClientHandler()
            {
                AutomaticDecompression =
                    DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

        protected Creator _creator;
        protected SwanConnection _connection;
        protected SwanConnection _badAccessNodeConnection;
        protected SwanConnection _badAccessKeyConnection;
        protected SwanConnection _badRsaConnection;

        [TestInitialize]
        public void TestInitialize()
        {
            _creator = new Creator(
                OwidDomain,
                new RSACryptoServiceProvider(512));
            _connection = new SwanConnection(
                Scheme,
                AccessNode,
                AccessKey,
                _creator,
                _operation);
            _badAccessNodeConnection = new SwanConnection(
                Scheme,
                BadAccessNode,
                AccessKey,
                _creator,
                _operation);
            _badAccessKeyConnection = new SwanConnection(
                Scheme,
                AccessNode,
                BadAccessKey,
                _creator,
                _operation);
            _badRsaConnection = new SwanConnection(
                Scheme,
                AccessNode,
                AccessKey,
                _operation);
        }

        /// <summary>
        /// Call the redirect URL as if the browser was performing the
        /// request and get the encrypted string from the response.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected static async Task<Uri> GetEncryptedData(Uri uri)
        {
            var html = await _client.GetStringAsync(uri);
            var encryptUrl = _uriRegex.Match(html);
            Assert.IsNotNull(encryptUrl);
            Assert.IsTrue(encryptUrl.Success);
            Assert.IsTrue(Uri.TryCreate(
                encryptUrl.Value,
                UriKind.Absolute,
                out uri));
            return uri;
        }
    }
}
