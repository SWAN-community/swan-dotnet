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

using System.Collections.Generic;

namespace Swan.Client.Model
{
    /// <summary>
    /// Fetch operation to retrieve the SWAN data for use with a call to Decrypt or
    /// DecryptRaw.
    /// </summary>
    public class Fetch : Operation
    {
        /// <summary>
        // Existing SWAN data pairs
        /// </summary>
        public Pair[] Existing { get; set; }

        internal Fetch(ISwanConnection connection, Operation operation) 
            : base(connection, operation)
        {
        }

        internal override void SetData(
            List<KeyValuePair<string, string>> parameters)
        {
            base.SetData(parameters);
            foreach (var existing in Existing)
            {
                switch (existing.Key)
                {
                    case "swid":
                    case "pref":
                        // TODO: Validate the value is an OWID.
                        parameters.Set(existing.Key, existing.Value);
                        break;
                }
            }
        }
    }
}
