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
using System.Text.Json.Serialization;

namespace Swan.Client.Model
{
    /// Operation has members for all the parameters for a storage operation
    /// involving a URL that is requested by the web browser.
    public class Operation : Client
    {
		/// <summary>
		/// The URL to return to with the encrypted data appended to it.
		/// </summary>
		[JsonPropertyName("returnUrl")]
		public string ReturnUrl { get; set; }

		/// <summary>
		/// The access node that will be used to decrypt the result of the 
		/// storage operation. Defaults to the access node that started the 
		/// storage operation.
		/// </summary>
		[JsonPropertyName("accessNode")]
		public string AccessNode { get; set; }

		/// <summary>
		/// The title of the progress UI page.
		/// </summary>
		[JsonPropertyName("title")]
		public string Title { get; set; }

		/// <summary>
		/// The text of the message in the progress UI.
		/// </summary>
		[JsonPropertyName("message")]
		public string Message { get; set; }

		/// <summary>
		/// The HTML color for the progress indicator.
		/// </summary>
		[JsonPropertyName("progressColor")]
		public string ProgressColor { get; set; }

		/// <summary>
		/// The HTML color for the progress UI background.
		/// </summary>
		[JsonPropertyName("backgroundColor")]
		public string BackgroundColor { get; set; }

		/// <summary>
		/// The HTML color for the message text.
		[JsonPropertyName("messageColor")]
		public string MessageColor { get; set; }

		/// <summary>
		/// Number of storage nodes to use for operations.
		/// </summary>
		[JsonPropertyName("nodeCount")]
		public int NodeCount { get; set; }

		/// <summary>
		/// DisplayUserInterface true if a progress UI should be displayed during the
		/// storage operation, otherwise false.
		/// </summary>
		[JsonPropertyName("displayUserInterface")]
		public bool DisplayUserInterface { get; set; }

		/// <summary>
		/// PostMessageOnComplete true if at the end of the operation the resulting
		/// data should be returned to the parent using JavaScript postMessage,
		/// otherwise false. Default false.
		/// </summary>
		[JsonPropertyName("postMessageOnComplete")]
		public bool PostMessageOnComplete { get; set; }

		/// <summary>
		/// UseHomeNode true if the home node can be used if it contains current
		/// data. False if the SWAN network should be consulted irrespective of the
		/// state of data held on the home node. Default true.
		/// </summary>
		[JsonPropertyName("useHomeNode")]
		public bool UseHomeNode { get; set; }

		/// <summary>
		/// JavaScript true if the response for storage operations should be
		/// JavaScript include that will continue the operation. This feature
		/// requires cookies to be sent for DOM inserted JavaScript elements. Default
		/// false.
		/// </summary>
		[JsonPropertyName("javaScript")]
		public bool JavaScript { get; set; }

		/// <summary>
		/// Optional array of strings that can be used to pass state information to
		/// the party that retrieves the results of the storage operation. For
		/// example; passing information between a Publisher and User Interface
		/// Provider such as a CMP in the storage operation.
		/// </summary>
		[JsonPropertyName("state")]
		public string[] State { get; set; }

		public Operation() : base()
		{
		}

		internal Operation(IConnection connection): base (connection)
		{
		}

		internal Operation(IConnection connection, Operation operation) 
			: this(connection)
        {
			AccessNode = operation.AccessNode;
			BackgroundColor = operation.BackgroundColor;
			DisplayUserInterface = operation.DisplayUserInterface;
			JavaScript = operation.JavaScript;
			Message = operation.Message;
			MessageColor = operation.MessageColor;
			NodeCount = operation.NodeCount;
			PostMessageOnComplete = operation.PostMessageOnComplete;
			ProgressColor = operation.ProgressColor;
			ReturnUrl = operation.ReturnUrl;
			State = operation.State;
			Title = operation.Title;
			UseHomeNode = operation.UseHomeNode;
		}

        internal override void SetData(
			List<KeyValuePair<string, string>> parameters)
        {
			base.SetData(parameters);
			if (String.IsNullOrEmpty(ReturnUrl)) 
			{
				throw new ArgumentException("ReturnURL required");
			}
			Uri returnUrl;
			if (Uri.TryCreate(ReturnUrl, UriKind.Absolute, out returnUrl) == false)
            {
				throw new ArgumentException("ReturnURL not a valid URL");
			}
			parameters.Set("returnUrl", returnUrl.ToString());
			if (String.IsNullOrEmpty(AccessNode) == false)
			{
				parameters.Set("accessNode", AccessNode);
			}
			if (String.IsNullOrEmpty(Title) == false) 
			{
				parameters.Set("title", Title);
			}
			if (String.IsNullOrEmpty(Message) == false){
				parameters.Set("message", Message);
			}
			if (String.IsNullOrEmpty(ProgressColor) == false)
			{
				parameters.Set("progressColor", ProgressColor);
			}
			if (String.IsNullOrEmpty(BackgroundColor) == false)
			{
				parameters.Set("backgroundColor", BackgroundColor);
			}
			if (String.IsNullOrEmpty(MessageColor) == false)
			{
				parameters.Set("messageColor", MessageColor);
			}
			if (NodeCount != 0) {
				parameters.Set("nodeCount", NodeCount.ToString());
			}
			parameters.Set("displayUserInterface", 
				DisplayUserInterface.ToString());
			parameters.Set("postMessageOnComplete",
				PostMessageOnComplete.ToString());
			parameters.Set("useHomeNode", UseHomeNode.ToString());
			parameters.Set("javaScript", JavaScript.ToString());
			if (State != null)
			{
				foreach (var value in State)
				{
					parameters.Add("state", value);
				}
			}
		}
	}
}
