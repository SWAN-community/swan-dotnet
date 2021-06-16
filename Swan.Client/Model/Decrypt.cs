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

        internal Decrypt(IConnection connection) : base(connection)
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