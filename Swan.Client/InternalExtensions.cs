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
using Microsoft.Extensions.Primitives;
using Swan.Client.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Swan.Client
{
    internal static class InternalExtensions
    {
        private static readonly HttpClientHandler GzipHttpHandler = 
            new HttpClientHandler()
        {
            AutomaticDecompression = 
                DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        internal static async Task<HttpResponseMessage> Request(
            this Base value, 
            string action, 
            IList<KeyValuePair<string, string>> parameters)
        {
            // Verify the provided parameters.
            if (String.IsNullOrEmpty(value.Connection.Scheme))
            {
                throw new ArgumentException("scheme must be provided");
            }
            if (String.IsNullOrEmpty(value.Connection.AccessNode))
            {
                throw new ArgumentException("operator must be provided");
            }
            if (String.IsNullOrEmpty(value.Connection.AccessKey))
            {
                throw new ArgumentException("accessKey must be provided");
            }

            // Construct the SWAN URL.
            UriBuilder u = new UriBuilder(
                value.Connection.Scheme,
                value.Connection.AccessNode);
            u.Path = "/swan/api/v1/" + action;

            // Add the access key to the data.
            parameters.Set("accessKey", value.Connection.AccessKey);

            // Connect to the SWAN operator and return the response.
            var client = new HttpClient(GzipHttpHandler);
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(u.Uri, content);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(
                    await response.Content.ReadAsStringAsync(),
                    null,
                    response.StatusCode);
            }
            return response;
        }

        internal static async Task<Stream> RequestAsStream(
            this Base model,
            string action,
            IList<KeyValuePair<string, string>> parameters)
        {
            var response = await model.Request(action, parameters);
            return await response.Content.ReadAsStreamAsync();
        }

        internal static async Task<byte[]> RequestAsByteArray(
            this Base model,
            string action,
            IList<KeyValuePair<string, string>> parameters)
        {
            var response = await model.Request(action, parameters);
            return await response.Content.ReadAsByteArrayAsync();
        }

        internal static async Task<string> RequestAsString(
            this Base model,
            string action,
            IList<KeyValuePair<string, string>> parameters)
        {
            var response = await model.Request(action, parameters);
            return await response.Content.ReadAsStringAsync();
        }

        internal static void Set(
            this IList<KeyValuePair<string, string>> list, 
            string key, 
            string value)
        {
            var index = 0;
            while (index < list.Count)
            {
                if (list[index].Key.Equals(key))
                {
                    list.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
            list.Add(new KeyValuePair<string, string>(key, value));
        }

        internal static void Add(
            this IList<KeyValuePair<string, string>> list,
            string key,
            string value)
        {
            list.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// SetHomeNodeHeaders adds the HTTP headers from the request that are relevant
        /// to the calculation of the home node to the values collection.
        /// </summary>
        /// <param name="request"></param>
        internal static void SetHomeNodeHeaders(
            this HttpRequest request,
            List<KeyValuePair<string, string>> parameters)
        {
            StringValues values;
            if (request.Headers.TryGetValue("X-Forwarded-For", out values))
            {
                parameters.Set("X-Forwarded-For", values.First());
            }
            if (request.HttpContext.Connection.RemoteIpAddress != null)
            {
                parameters.Set(
                    "remoteAddr",
                    request.HttpContext.Connection.RemoteIpAddress.ToString());
            }
        }
    }
}
