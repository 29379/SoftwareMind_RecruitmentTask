namespace HotDeskBookingSystem.Exceptions
{
    [Serializable]
    public class BadCredentialsException : Exception
    {
        public BadCredentialsException() { }
        public BadCredentialsException(string message) : base(message) { }
        public BadCredentialsException(string message, Exception inner) : base(message, inner) { }
        protected BadCredentialsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
