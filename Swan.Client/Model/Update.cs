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
using System.Collections.Generic;
using System.Text;
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
        /// <summary>
        /// Crypto provider to use when signing OWIDs.
        /// </summary>
        private readonly Creator _creator;

        [JsonPropertyName("swid")]
        public string Swid
        {
            get
            {
                return _swid.AsBase64();
            }
            set
            {
                _swid = new Owid.Client.Model.Owid(value);
            }
        }
        private Owid.Client.Model.Owid _swid;

        [JsonIgnore]
        public string SwidAsOwidString => _swid.AsBase64();
        
        [JsonIgnore]
        public Owid.Client.Model.Owid SwidAsOwid => _swid;
        
        [JsonPropertyName("pref")]
        public string Pref
        {
            get
            {
                return ASCIIEncoding.ASCII.GetString(_pref.Payload);
            }
            set
            {
                _pref = new Owid.Client.Model.Owid();
                _pref.Payload = ASCIIEncoding.ASCII.GetBytes(value);
                if (_creator != null)
                {
                    _creator.Sign(_pref);
                }
            }
        }
        private Owid.Client.Model.Owid _pref;

        [JsonIgnore]
        public string PrefAsOwidString => _pref.AsBase64();

        [JsonIgnore]
        public Owid.Client.Model.Owid PrefAsOwid => _pref;
        
        [JsonPropertyName("email")]
        public string Email 
        {
            get
            {
                return ASCIIEncoding.ASCII.GetString(_email.Payload);
            }
            set
            {
                _email = new Owid.Client.Model.Owid();
                _email.Payload = ASCIIEncoding.ASCII.GetBytes(value);
                if (_creator != null)
                {
                    _creator.Sign(_email);
                }
            }
        }
        private Owid.Client.Model.Owid _email;

        [JsonIgnore]
        public string EmailAsOwidString => _email.AsBase64();

        [JsonIgnore]
        public Owid.Client.Model.Owid EmailAsOwid => _email;
        
        [JsonPropertyName("salt")]
        public string Salt
        {
            get
            {
                return ASCIIEncoding.ASCII.GetString(_salt.Payload);
            }
            set
            {
                _salt = new Owid.Client.Model.Owid();
                _salt.Payload = ASCIIEncoding.ASCII.GetBytes(value);
                if (_creator != null)
                {
                    _creator.Sign(_salt);
                }
            }
        }
        private Owid.Client.Model.Owid _salt;

        [JsonIgnore]
        public string SaltAsOwidString => _salt.AsBase64();

        [JsonIgnore]
        public Owid.Client.Model.Owid SaltAsOwid => _salt;
        
        public Update() : base() { }

        public Update(Creator creator) : base()
        {
            ValidateCreator(creator);
            _creator = creator;
        }

        public Update(Creator creator, Operation operation) : base(operation)
        {
            ValidateCreator(creator);
            _creator = creator;
        }

        public Update(ISwanConnection connection, Creator creator) 
            : base(connection)
        {
            ValidateCreator(creator);
            _creator = creator;
        }

        public Update(
            ISwanConnection connection,
            Creator creator,
            Operation operation)
            : base(connection, operation)
        {
            ValidateCreator(creator);
            _creator = creator;
        }

        public Update(
            ISwanConnection connection, 
            Creator creator, 
            Update source)
            : this(connection, creator, (Operation)source)
        {
            this._swid = source._swid;
            this._pref = source._pref;
            this._email = source._email;
            this._salt = source._salt;
        }

        internal override void SetData(
            List<KeyValuePair<string, string>> parameters)
        {
            base.SetData(parameters);
            if (_swid != null)
            {
                parameters.Add("swid", SwidAsOwidString);
            }
            if (_pref != null)
            {
                parameters.Add("pref", PrefAsOwidString);
            }
            if (_email != null)
            {
                parameters.Add("email", EmailAsOwidString);
            }
            if (_salt != null)
            {
                parameters.Add("salt", SaltAsOwidString);
            }
        }

        private static void ValidateCreator(Creator creator)
        {
            if (creator == null)
            {
                throw new ArgumentException(
                    "Connection does not support update operations because " +
                    "an Creator is not available", 
                    "creator");
            }
        }
    }
}