using System.Collections.Generic;

namespace Swan.Client.Model
{
    /// <summary>
    /// Base is the base structure for all actions. It includes the scheme for 
    /// the SWAN Operator URLs, the Operator domain and the access key needed 
    /// by the SWAN Operator.
    /// </summary>
    public class Base
    {
        internal readonly IConnection Connection;

        internal Base()
        { }

        internal Base(IConnection connection)
        {
            Connection = connection;
        }
    }
}
