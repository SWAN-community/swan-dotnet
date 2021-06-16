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

using Owid.Client;
using System;
using System.Text.Json.Serialization;

namespace Swan.Client.Model
{
    /// <summary>
    /// Update operation from a User Interface Provider where the preferences, email
    /// and salt have been captured. The SWID is returned from a previous call to
    /// swan.CreateSWID.
    /// </summary>
    public class Update : Operation
    {
        [JsonIgnore]
        public Owid.Client.Model.Owid Swid { get; set; } = null;

        [JsonIgnore]
        public Owid.Client.Model.Owid Pref { get; set; } = null;

        [JsonIgnore]
        public Owid.Client.Model.Owid Email { get; set; } = null;

        [JsonIgnore]
        public Owid.Client.Model.Owid Salt { get; set; } = null;

        [JsonPropertyName("swid")]
        public string SwidAsString
        {
            get
            {
                return Swid.AsBase64();
            }
            set
            {
                Swid = String.IsNullOrEmpty(value) ?
                    new Owid.Client.Model.Owid() :
                    new Owid.Client.Model.Owid(value);
            }
        }

        [JsonPropertyName("pref")]
        public string PrefAsString
        {
            get
            {
                return Pref.AsBase64();
            }
            set
            {
                Pref = String.IsNullOrEmpty(value) ?
                    new Owid.Client.Model.Owid() :
                    new Owid.Client.Model.Owid(value);
            }
        }

        [JsonPropertyName("email")]
        public string EmailAsString
        {
            get
            {
                return Email.AsBase64();
            }
            set
            {
                Email = String.IsNullOrEmpty(value) ?
                    new Owid.Client.Model.Owid() :
                    new Owid.Client.Model.Owid(value);
            }
        }

        [JsonPropertyName("salt")]
        public string SaltAsString
        {
            get
            {
                return Salt.AsBase64();
            }
            set
            {
                Salt = String.IsNullOrEmpty(value) ?
                    new Owid.Client.Model.Owid() :
                    new Owid.Client.Model.Owid(value);
            }
        }

        public Update() : base() { }

        public Update(IConnection connection) : base(connection)
        {
        }

        internal Update(IConnection connection, Operation operation) 
            : base(connection, operation)
        {
        }
    }
}
