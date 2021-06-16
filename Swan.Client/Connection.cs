using Microsoft.AspNetCore.Http;
using Swan.Client.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swan.Client
{
    public class Connection : IConnection
    {
        /// <summary>
        /// The HTTP or HTTPS scheme to use for SWAN requests
        /// </summary>
        public string Scheme { get; }

        /// <summary>
        /// Domain name of the SWAN Operator access node
        /// </summary>
        public string AccessNode { get; }

        /// <summary>
        /// SWAN access key provided by the SWAN Operator
        /// </summary>
        public string AccessKey { get; }

        private readonly Operation _operation = null;
    
        public Connection(string scheme, string accessNode, string accessKey)
            : this (scheme, accessNode, accessKey, new Operation())
        {
        }

        public Connection(
            string scheme, 
            string accessNode, 
            string accessKey, 
            Operation operation)
        {
            Scheme = scheme;
            AccessNode = accessNode;
            AccessKey = accessKey;
            _operation = operation;
        }

        public Model.Client NewClient(HttpRequest request)
        {
            return new Model.Client(this)
            {
                Request = request
            };
        }

        public Decrypt NewDecrypt(string encrypted)
        {
            return new Decrypt(this)
            {
               Encrypted = encrypted
            };
        }

        /// <summary>
        /// Decrypt returns SWAN key value pairs for the data contained in the
        /// encrypted string.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public async Task<Pair[]> Decrypt(string encrypted)
        {
            return await NewDecrypt(encrypted).Decrypt();
        }

        /// <summary>
        /// DecryptRaw returns key value pairs for the raw SWAN data contained 
        /// in the encrypted string. Must only be used by User Interface 
        /// Providers.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public async Task<Update> DecryptRaw(
            string encrypted)
        {
            return await NewDecrypt(encrypted).DecryptRaw();
        }

        public Fetch Fetch(
            HttpRequest request, 
            string returnUrl, 
            IEnumerable<Pair> existing = null)
        {
            return new Fetch(this, _operation)
            {
                Request = request,
                ReturnUrl = returnUrl,
                Existing = existing != null ? 
                    existing.ToArray() : 
                    new Pair[] { }
            };
        }

        public Stop NewStop(HttpRequest request, string returnUrl, string host)
        {
            return new Stop(this, _operation)
            {
                Request = request,
                ReturnUrl = returnUrl,
                Host = host
            };
        }

        public Update NewUpdate(HttpRequest request, string returnUrl)
        {
            return new Update(this, _operation)
            {
                Request = request,
                ReturnUrl = returnUrl
            };
        }
    }
}
