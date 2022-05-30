

namespace AudioObjectLib
{
    [System.Serializable]
    public class AudioData
    {
    public float FXVolume { get; set; } = 0.5f;
    public float MusicVolume { get; set; } = 0.2f;
    public bool MusicEnabled { get; set; } = true;

    public AudioData ()
    {

    }
}
}
