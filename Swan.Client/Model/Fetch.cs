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

        internal Fetch(IConnection connection, Operation operation) 
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
