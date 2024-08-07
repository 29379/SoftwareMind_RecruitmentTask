namespace HotDeskBookingSystem.Exceptions
{
    [Serializable]
    public class UnsupportedOperationException : Exception
    {
        public UnsupportedOperationException() { }
        public UnsupportedOperationException(string message) : base(message) { }
        public UnsupportedOperationException(string message, Exception innerException) : base(message, innerException) { }
        protected UnsupportedOperationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
