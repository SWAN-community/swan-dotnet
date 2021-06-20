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
    /// Stop operation to block an advert domain or identifier.
    /// </summary>
    public class Stop : Operation
    {
        /// <summary>
        /// Advert host to block
        /// </summary>
        public string Host { get; set; }

        public Stop(ISwanConnection connection) : base(connection)
        {
        }

        internal Stop(ISwanConnection connection, Operation operation)
            : base(connection, operation)
        {
        }

        internal override void SetData(
            List<KeyValuePair<string, string>> parameters)
        {
            base.SetData(parameters);
            parameters.Set("host", Host);
        }
    }
}