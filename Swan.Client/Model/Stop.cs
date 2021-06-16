namespace Swan.Client.Model
{
    /// <summary>
    /// Stop operation to block an advert domain or identifier.
    /// </summary>
    public class Stop : Operation
    {
        /// <summary>
        /// Advert host to block
        /// </summary>
        public string Host { get; set; }

        public Stop(IConnection connection) : base(connection)
        {
        }

        internal Stop(IConnection connection, Operation operation)
            : base(connection, operation)
        {
        }
    }
}