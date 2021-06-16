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
