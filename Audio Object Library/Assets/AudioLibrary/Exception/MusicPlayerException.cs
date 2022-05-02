namespace AudioObjectLib
{
[System.Serializable]
public class MusicPlayerException : System.Exception
{
    public MusicPlayerException() { }
    public MusicPlayerException(string message) : base(message) { }
    public MusicPlayerException(string message, System.Exception inner) : base(message, inner) { }
    protected MusicPlayerException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}    
}
