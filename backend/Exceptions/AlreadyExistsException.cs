namespace HotDeskBookingSystem.Exceptions
{
    [Serializable]
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() { }
        public AlreadyExistsException(string message) : base(message) { }
        public AlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected AlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
