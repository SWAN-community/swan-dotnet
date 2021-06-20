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

using System.Text.Json;
using Swan.Client.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;

namespace Swan.Client
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the last Base64 characters from a string.
        /// </summary>
        private static readonly Regex _base64End = new Regex(
            @"[a-zA-Z\d_-]+$", RegexOptions.Compiled);

        /// <summary>
        /// GetURL contacts the SWAN operator domain with the access key and 
        /// returns a URL string that the web browser should be immediately 
        /// directed to.
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetURL(this Fetch fetch)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            fetch.SetData(parameters);
            return await fetch.RequestAsString("fetch", parameters);
        }

        /// <summary>
        /// GetURL contacts the SWAN operator domain with the access key and 
        /// returns a URL string that the web browser should be directed to.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static async Task<string> GetURL(this Update update)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            update.SetData(parameters);
            return await update.RequestAsString("update", parameters);
        }

        /// <summary>
        /// GetURL contacts the SWAN operator domain with the access key and 
        /// returns a URL string that the web browser should be directed to.
        /// </summary>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static async Task<string> GetURL(this Stop stop)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            stop.SetData(parameters);
            return await stop.RequestAsString("stop", parameters);
        }

        public static async Task<Pair[]> Decrypt(this Decrypt decrypt)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            decrypt.SetData(parameters);
            var stream = await decrypt.RequestAsStream(
                "decrypt",
                parameters);
            return await JsonSerializer.DeserializeAsync<Pair[]>(stream);
        }

        public static async Task<Update> 
            DecryptRaw(this Decrypt decrypt)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            decrypt.SetData(parameters);
            var stream = await decrypt.RequestAsStream(
                "decrypt-raw",
                parameters);
            return await JsonSerializer.DeserializeAsync<Update>(stream);
        }

        public static async Task<string> HomeNode(this Model.Client client)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            client.SetData(parameters);
            return await client.RequestAsString(
                "home-node",
                parameters);
        }

        public static async Task<Owid.Client.Model.Owid> CreateSwid(
            this ISwanConnection connection)
        {
            var byteArray = await new Base(connection).RequestAsByteArray(
                "create-swid", 
                new List<KeyValuePair<string, string>>());
            return new Owid.Client.Model.Owid(byteArray);
        }

        /// <summary>
        /// Returns the encrypted part of the Uri.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetEncrypted(this Uri uri)
        {
            var match = _base64End.Match(uri.OriginalString);
            if (match.Success)
            {
                return match.Value;
            }
            return null;
        }

        /// <summary>
        /// Returns the string that has been appended to the return Url.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public static string GetEncrypted(this Uri uri, string returnUrl)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            if (returnUrl == null)
            {
                throw new ArgumentNullException("returnUrl");
            }
            if (uri.OriginalString.StartsWith(returnUrl) == false)
            {
                throw new ArgumentException(
                    "Uri must start with returnUrl", 
                    "uri");
            }
            return uri.OriginalString.Substring(returnUrl.Length);
        }
    }
}