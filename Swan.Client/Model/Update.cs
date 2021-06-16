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
