/* ****************************************************************************
 * Copyright 2021 51 Degrees Mobile Experts Limited (51degrees.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 * ***************************************************************************/

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
