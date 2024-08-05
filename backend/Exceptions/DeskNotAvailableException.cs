namespace HotDeskBookingSystem.Exceptions
{
    [Serializable]
    public class DeskNotAvailableException : Exception
    {
        public DeskNotAvailableException() { }
        public DeskNotAvailableException(string message) : base(message) { }
        public DeskNotAvailableException(string message, Exception inner) : base(message, inner) { }
        protected DeskNotAvailableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
