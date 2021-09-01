using Owid.Client.Model.Configuration;

namespace Swan.Client.Model.Configuration
{ 
    public class SwanConfiguration
    {
        /// <summary>
        /// The HTTP or HTTPS scheme to use for SWAN requests
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Domain name of the SWAN Operator access node
        /// </summary>
        public string AccessNode { get; set; }

        /// <summary>
        /// SWAN access key provided by the SWAN Operator
        /// </summary>
        public string AccessKey { get; set; }
    }
}
