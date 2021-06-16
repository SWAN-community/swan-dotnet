using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swan.Client.Model;
using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Owid.Client;

namespace Swan.Client.Test
{
    [TestClass]
    public class Actions
    {
        private const string Scheme = "https";
        private const string AccessNode = "51db.uk";
        private const string BadAccessNode = "51dz.uk";
        private const string AccessKey = "CMPKeySWAN";
        private const string BadAccessKey = "_CMPKeySWAN";
        private const string ReturnUrl = "https://test.com/index.html";
        private const string BadReturnUrl = "http*://test.com/index.html";
        private static readonly Operation _operation = new Operation()
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

        private static readonly Regex _uriRegex = new Regex(
            @"http[s]://[^\""']+",
            RegexOptions.Compiled);
        private static readonly HttpContext _httpContext = 
            new DefaultHttpContext();
        private static readonly HttpClient _client = new HttpClient(
            new HttpClientHandler()
            {
                AutomaticDecompression =
                    DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

        private Connection _connection;
        private Connection _badAccessNodeConnection;
        private Connection _badAccessKeyConnection;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new Connection(
                Scheme,
                AccessNode,
                AccessKey,
                _operation);
            _badAccessNodeConnection = new Connection(
                Scheme,
                BadAccessNode,
                AccessKey,
                _operation);
            _badAccessKeyConnection = new Connection(
                Scheme,
                AccessNode,
                BadAccessKey,
                _operation);
        }

        [TestMethod]
        public async Task TestFetch()
        {
            var first = await _connection.Fetch(
                _httpContext.Request, 
                ReturnUrl).GetURL();
            Assert.IsNotNull(first);
            var second = await _connection.Fetch(
                _httpContext.Request,
                ReturnUrl).GetURL();
            Assert.IsNotNull(second);
            Assert.AreNotEqual(first, second);
        }

        [TestMethod]
        public void TestFetchNoReturnUrl()
        {
            Assert.ThrowsException<AggregateException>(() =>
                 _connection.Fetch(
                _httpContext.Request,
                "").GetURL().Result);
        }

        [TestMethod]
        public void TestFetchNoRequest()
        {
            Assert.ThrowsException<AggregateException>(() =>
                 _connection.Fetch(
                null,
                ReturnUrl).GetURL().Result);
        }

        [TestMethod]
        public void TestFetchBadReturnUrl()
        {
            Assert.ThrowsException<AggregateException>(() =>
                 _connection.Fetch(
                _httpContext.Request,
                BadReturnUrl).GetURL().Result);
        }

        [TestMethod]
        public void TestFetchBadAccessNode()
        {
            Assert.ThrowsException<AggregateException>(() =>
                 _badAccessNodeConnection.Fetch(
                _httpContext.Request,
                ReturnUrl).GetURL().Result);
        }

        [TestMethod]
        public void TestFetchBadAccessKey()
        {
            Assert.ThrowsException<AggregateException>(() =>
                _badAccessKeyConnection.Fetch(
                _httpContext.Request,
                ReturnUrl).GetURL().Result);
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            var first = await _connection.NewUpdate(
                _httpContext.Request,
                ReturnUrl).GetURL();
            Assert.IsNotNull(first);
            var second = await _connection.NewUpdate(
                _httpContext.Request,
                ReturnUrl).GetURL();
            Assert.IsNotNull(second);
            Assert.AreNotEqual(first, second);
        }

        [TestMethod]
        public async Task TestDecrypt()
        {
            var pairs = await _connection.Decrypt(await GetEncryptedData());
            Assert.IsNotNull(pairs);
        }

        [TestMethod]
        public async Task TestDecryptRaw()
        {
            var update = await _connection.DecryptRaw(await GetEncryptedData());
            Assert.IsNotNull(update);
            Assert.IsNotNull(update.SwidAsString);
        }

        [TestMethod]
        public async Task TestSwid()
        {
            var update = await _connection.DecryptRaw(await GetEncryptedData());
            var success = await update.Swid.VerifyAsync();
            Assert.IsTrue(success);
        }

        /// <summary>
        /// Completes a SWAN fetch storage operation without using a web 
        /// browser to return the values passed into the fetch method. Returns
        /// the encrypted result for use in decrypt operations.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetEncryptedData()
        {
            // Get the redirect URL.
            var fetch = _connection.Fetch(
                _httpContext.Request,
                ReturnUrl);
            var redirectUrl = await fetch.GetURL();
            Assert.IsNotNull(redirectUrl);
            Uri uri;
            Assert.IsTrue(Uri.TryCreate(redirectUrl, UriKind.Absolute, out uri));

            // Call the redirect URL as if the browser was performing the
            // request and get the encrypted string from the response.
            var response = await _client.GetAsync(uri);
            var html = await response.Content.ReadAsStringAsync();
            var encryptUrl = _uriRegex.Match(html);
            Assert.IsNotNull(encryptUrl);
            Assert.IsTrue(encryptUrl.Success);
            Assert.IsTrue(Uri.TryCreate(
                encryptUrl.Value,
                UriKind.Absolute,
                out uri));
            return uri.GetEncrypted(fetch.ReturnUrl);
        }
    }
}
