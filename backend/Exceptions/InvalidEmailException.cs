namespace HotDeskBookingSystem.Exceptions
{
    [Serializable]
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() { }
        public InvalidEmailException(string message) : base(message) { }
        public InvalidEmailException(string message, Exception inner) : base(message, inner) { }
        protected InvalidEmailException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
