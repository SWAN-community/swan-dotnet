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
using Swan.Client.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Swan.Client
{
    public interface ISwanConnection
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
            IEnumerable<Pair> existing = null);

        /// <summary>
        /// NewUpdate creates a new fetch operation using the default in the 
        /// connection.
        /// </summary>
        /// <param name="request">http request from a web browser</param>
        /// <param name="returnUrl">
        /// return URL after the operation completes
        /// </param>
        /// <returns></returns>
        Update Update(HttpRequest request, string returnUrl);

        // <summary>
        /// NewUpdate creates a new fetch operation using the values contained
        /// in the source update object.
        /// </summary>
        /// <param name="request">http request from a web browser</param>
        /// <param name="source">
        /// Values of properties to use for the new update operation.
        /// <returns>
        /// A new update operation connected to the connection.
        /// </returns>
        Update Update(HttpRequest request, Update source);

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
        Stop Stop(HttpRequest request, string returnUrl, string host);

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

        /// <summary>
        /// Decrypt returns SWAN key value pairs for the data contained in the
        /// encrypted string.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        Task<Pair[]> Decrypt(string encrypted);

        /// <summary>
        /// DecryptRaw returns key value pairs for the raw SWAN data contained 
        /// in the encrypted string. Must only be used by User Interface 
        /// Providers.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        Task<Update> DecryptRaw(string encrypted);
    }
}