using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Swan.Client.Model
{
    /// <summary>
    /// Client is used for actions where a request from a web browser is available.
    /// It is mainly used to set the home node from the public IP address of the web
    /// browser.
    /// </summary>
    public class Client : Base
    {
        /// <summary>
        /// The HTTP request from the web browser
        /// </summary>
        public HttpRequest Request { get; set; }

        protected Client() : base()
        { 
        }

        public Client(IConnection connection) : base (connection)
        {
        }

        internal virtual void SetData(
            List<KeyValuePair<string, string>> parameters)
        {
            if (Request == null) 
            {
                throw new ArgumentException("Request required");
            }
            Request.SetHomeNodeHeaders(parameters);
        }
    }
}
