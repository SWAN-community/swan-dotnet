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
using Owid.Client;
using Swan.Client.Model;
using Swan.Client.Model.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Swan.Client
{
    public class SwanConnection : ISwanConnection
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

        /// <summary>
        /// Template details for any new storage operation.
        /// </summary>
        private readonly Operation _operation = null;

        /// <summary>
        /// Creator used with <see cref="Model.Update"/> update actions. If not 
        /// provided then update actions will throw an exception.
        /// </summary>
        private readonly Creator _creator;

        public SwanConnection(SwanConfiguration configuration)
            : this (
                  configuration.Scheme,
                  configuration.AccessNode,
                  configuration.AccessKey,
                  new Creator(configuration.OwidConfiguration))
        {
        }


        public SwanConnection(
            string scheme, 
            string accessNode,
            string accessKey,
            Creator creator)
            : this (scheme, accessNode, accessKey, creator, new Operation())
        {
        }

        public SwanConnection(
            string scheme, 
            string accessNode, 
            string accessKey,
            Creator creator,
            Operation operation)
        {
            Scheme = scheme;
            AccessNode = accessNode;
            AccessKey = accessKey;
            _creator = creator;
            _operation = operation;
        }

        public SwanConnection(
            string scheme,
            string accessNode,
            string accessKey)
            : this(scheme, accessNode, accessKey, null, new Operation())
        {
        }

        public SwanConnection(
            string scheme,
            string accessNode,
            string accessKey,
            Operation operation)
            : this (scheme, accessNode, accessKey, null, operation)
        { 
        }

        public Model.Client NewClient(HttpRequest request)
        {
            return new Model.Client(this)
            {
                Request = request
            };
        }

        /// <summary>
        /// NewDecrypt creates a new decrypt request using the default in the
        /// connection.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
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

        public Stop Stop(HttpRequest request, string returnUrl, string host)
        {
            return new Stop(this, _operation)
            {
                Request = request,
                ReturnUrl = returnUrl,
                Host = host
            };
        }

        public Update Update(HttpRequest request, Update source)
        {
            return new Update(this, _creator, source)
            {
                Request = request,
            };
        }

        public Update Update(HttpRequest request, string returnUrl)
        {
            return new Update(this, _creator, _operation)
            {
                Request = request,
                ReturnUrl = returnUrl
            };
        }
    }
}
