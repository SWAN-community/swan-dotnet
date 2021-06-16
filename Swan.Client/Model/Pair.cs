using System;

namespace Swan.Client.Model
{
	/// <summary>
	/// Pair represents a key value pair stored in SWAN. The created and expiry times
	/// for the value are also available.
	/// </summary>
	public class Pair
    {
		/// <summary>
		/// The name of the key associated with the value
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// The UTC time when the value was created
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// The UTC time when the value will expire and should not be used
		/// </summary>
		public DateTime Expires { get; set; }

		/// <summary>
		/// The value for the key as a string
		/// </summary>
		public string Value { get; set; }
	}
}
