
namespace AudioObjectLib
{
[System.Serializable]
public class AudioDataManagerException : System.Exception
{
    public AudioDataManagerException() { }
    public AudioDataManagerException(string message) : base(message) { }
    public AudioDataManagerException(string message, System.Exception inner) : base(message, inner) { }
    protected AudioDataManagerException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}    
}
