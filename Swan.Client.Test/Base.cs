using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owid.Client;
using Swan.Client.Model;
using System.Security.Cryptography;

namespace Swan.Client.Test
{
    public abstract class Base
    {
        protected const string Scheme = "https";
        protected const string AccessNode = "51db.uk";
        protected const string BadAccessNode = "51dz.uk";
        protected const string OwidDomain = "localhost";
        protected const string StopDomain = "stop.org";
        protected const string AccessKey = "CMPKeySWAN";
        protected const string BadAccessKey = "_CMPKeySWAN";
        protected const string ReturnUrl = "https://test.com/index.html?";
        protected const string BadReturnUrl = "http*://test.com/index.html?";
        protected const string Payload = "Hello World";
        protected const string Email = "test@localhost.com";
        protected const string Salt = "AAA=";
        protected const string Preference = "on";
        protected static readonly Operation _operation = new Operation()
        {
            BackgroundColor = "white",
            DisplayUserInterface = true,
            JavaScript = false,
            Message = "Testing is essential",
            MessageColor = "black",
            NodeCount = 1, // Important to avoid multiple redirections in the test.
            PostMessageOnComplete = false,
            ProgressColor = "blue",
            Title = "Testing",
            UseHomeNode = true
        };

        public static readonly HttpContext _httpContext =
            new DefaultHttpContext();

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
    }
}
