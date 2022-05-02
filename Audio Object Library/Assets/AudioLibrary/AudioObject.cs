using System;
using System.Collections;
using UnityEngine;
namespace AudioObjectLib
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioObject : MonoBehaviour
    {
    [SerializeField] private AudioType typeAudio = AudioType.FX;

    private AudioSource audioSource;

    private AudioDataManager dataManager;

    public AudioType TypeAudio { get => typeAudio; }

    public event Action<AudioObject> onRemove;


    
   private void Awake() => Ini();

    private void Ini()
    {
        if (AudioDataManager.Manager == null)
        {
            throw new AudioObjectException("Audio manager not found");
        }


        dataManager = AudioDataManager.Manager;
        audioSource = GetComponent<AudioSource>();
        switch (typeAudio)
        {
            case AudioType.FX:
                dataManager.OnFXVolumeChanged += ChangeVolume;

                ChangeVolume(dataManager.GetVolumeFX());
                break;
            case AudioType.Music:
                dataManager.OnMusicVolumeChanged += ChangeVolume;

                ChangeVolume(dataManager.GetVolumeMusic());
                break;
            default:
                throw new AudioObjectException($"invalid type audio: {typeAudio}");
        }





    }




    private void ChangeVolume (float value)
    {

        audioSource.volume = value;
    }


    public void Remove()
    {
        Uncribe();
        Destroy(gameObject);

    }

    private void Uncribe()
    {
        if (dataManager != null)
        {
            switch (typeAudio)
            {
                case AudioType.FX:
                    dataManager.OnFXVolumeChanged -= ChangeVolume;
                    break;
                case AudioType.Music:
                    dataManager.OnMusicVolumeChanged -= ChangeVolume;
                    break;
                default:
                    throw new AudioObjectException($"invalid type audio: {typeAudio}");
            }
        }
    }

    public AudioSource GetAudioSource ()
    {
        if (audioSource == null)
        {
            Ini();
        }

        return audioSource;
    }


    private void OnDestroy()
    {
        try
        {
            onRemove?.Invoke(this);
            Uncribe();
        }
        catch
        {
        }

    }

    private IEnumerator RemoveWaitRealtime ()
    {
        float time = audioSource.clip.length + 0.01f;
#if UNITY_EDITOR
        Debug.Log($"{name} audio object destroy as {time} seconds");
#endif

        yield return new WaitForSecondsRealtime(time);
        Remove();
    }


    public void RemoveIFNotPlaying ()
    {
        Ini();
        StartCoroutine(RemoveWaitRealtime());
    }

}
}
