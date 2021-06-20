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

using System;
using System.Collections.Generic;

namespace Swan.Client.Model
{
    /// <summary>
    /// Decrypt contains the string to be decrypted via the call to SWAN.
    /// </summary>
    public class Decrypt : Base
    {
        /// <summary>
        /// The encrypted string to be decrypted by SWAN.
        /// </summary>
        public string Encrypted { get; set; }

        internal Decrypt(ISwanConnection connection) : base(connection)
        {
        }

        internal void SetData(
            List<KeyValuePair<string, string>> parameters)
        {
            if (String.IsNullOrEmpty(Encrypted))
            {
                throw new ArgumentException("Encrypted required");
            }
            parameters.Set("encrypted", Encrypted);
        }
    }
}