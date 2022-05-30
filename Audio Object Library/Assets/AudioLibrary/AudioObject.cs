using System;
using System.Collections;
using UnityEngine;
namespace AudioObjectLib
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioObject : MonoBehaviour
    {
    [SerializeField] private AudioType _typeAudio = AudioType.FX;

    private AudioSource _audioSource;

    private AudioDataManager _dataManager;
    public AudioType TypeAudio => _typeAudio;

    public event Action<AudioObject> OnRemove;

    private void Ini()
    {
        if (AudioDataManager.Manager == null)
        {
            throw new AudioObjectException("Audio manager not found");
        }


        _dataManager = AudioDataManager.Manager;
        _audioSource = GetComponent<AudioSource>();
        switch (_typeAudio)
        {
            case AudioType.FX:
                _dataManager.OnFXVolumeChanged += ChangeVolume;

                ChangeVolume(_dataManager.GetVolumeFX());
                break;
            case AudioType.Music:
                _dataManager.OnMusicVolumeChanged += ChangeVolume;

                ChangeVolume(_dataManager.GetVolumeMusic());
                break;
            default:
                throw new AudioObjectException($"invalid type audio: {_typeAudio}");
        }
    }

    public void Remove()
    {
        Uncribe();
        Destroy(gameObject);

    }

    private void Uncribe()
    {
        if (_dataManager != null)
        {
            switch (_typeAudio)
            {
                case AudioType.FX:
                    _dataManager.OnFXVolumeChanged -= ChangeVolume;
                    break;
                case AudioType.Music:
                    _dataManager.OnMusicVolumeChanged -= ChangeVolume;
                    break;
                default:
                    throw new AudioObjectException($"invalid type audio: {_typeAudio}");
            }
        }
    }

    public AudioSource GetAudioSource ()
    {
        if (_audioSource == null)
        {
            Ini();
        }

        return _audioSource;
    }


    private void OnDestroy()
    {
        try
        {
            OnRemove?.Invoke(this);
            Uncribe();
        }
        catch
        {
        }

    }

    private IEnumerator RemoveWaitRealtime ()
    {
        float time = _audioSource.clip.length + 0.01f;
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

     private void Awake() => Ini();

    private void ChangeVolume (float value) => _audioSource.volume = value;

}
}
