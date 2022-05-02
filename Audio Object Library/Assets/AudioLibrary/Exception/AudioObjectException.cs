namespace AudioObjectLib
{
    [System.Serializable]
    public class AudioObjectException : System.Exception
    {
        public AudioObjectException() { }
        public AudioObjectException(string message) : base(message) { }
        public AudioObjectException(string message, System.Exception inner) : base(message, inner) { }
        protected AudioObjectException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}