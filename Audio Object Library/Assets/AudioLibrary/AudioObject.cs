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

    private void Initialize()
    {
        if (AudioDataManager.Manager is null)
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

    public void Hide()
    {
     gameObject.SetActive(false);
    }

    public AudioSource GetAudioSource ()
    {
        if (_audioSource is null)
        {
            Initialize();
        }

        return _audioSource;
    }

    private IEnumerator RemoveWaitRealtime ()
    {

            float time = _audioSource.clip.length + 0.01f;

            yield return new WaitForSecondsRealtime(time);

#if UNITY_EDITOR
            Debug.Log($"{name} audio object ending play as {time} seconds");
#endif


            Hide();
    }


    public void RemoveIFNotPlaying ()
    {
        Initialize();

        StartCoroutine(RemoveWaitRealtime());
    }

     private void Awake() => Initialize();

    private void ChangeVolume (float value) => _audioSource.volume = value;

}
}
