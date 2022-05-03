using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AudioObjectLib
{
    public class AudioDataManager : MonoBehaviour
    {
    private static AudioDataManager _manager;

    private AudioData _audioData = new AudioData();


    public event Action<float> OnFXVolumeChanged;
    public event Action<float> OnMusicVolumeChanged;
    public event Action<bool> OnMusicEnabled;

    private const string PATH_PREFAB_AUDIO_OBJECT = "Prefabs/Audio/AudioObject";

    private AudioObject _audioObjectPrefab;

    public static AudioDataManager Manager { get => _manager; }

    
   private void Awake()
    {
        if (_manager == null)
        {
            _manager = this;
            _audioObjectPrefab = Resources.Load<AudioObject>(PATH_PREFAB_AUDIO_OBJECT);

            if (_audioObjectPrefab == null)
            {
                throw new AudioDataManagerException("audio object prefab not found");
            }
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

    }


    public float GetVolumeFX ()
    {
     return   _audioData.fxVolume;
    }

    public float GetVolumeMusic ()
    {
        return _audioData.musicVolume;
    }

    public bool GetMusicEnabled ()
    {
        return _audioData.musicEnabled;
    }

    public void SetVolumeFX (float value)
    {
        _audioData.fxVolume = ClampingVolume(value);
        OnFXVolumeChanged?.Invoke(_audioData.fxVolume);
    }

    public void SetVolumeMusic (float value)
    {
        _audioData.musicVolume = ClampingVolume(value);
        OnMusicVolumeChanged?.Invoke(_audioData.musicVolume);
    }

    private float ClampingVolume (float value)
    {
        return Mathf.Clamp(value, 0.0f, 1.0f);
    }

    public void SetEnabledMusic (bool status)
    {
        _audioData.musicEnabled = status;
        OnMusicEnabled?.Invoke(_audioData.musicEnabled);
    }

    public AudioObject CreateAudioObject (Vector3 position, AudioClip clip)
    {
        AudioObject audioObject = Instantiate(_audioObjectPrefab);
        audioObject.transform.position = position;
        audioObject.GetAudioSource().clip = clip;
        

        return audioObject;
    }


}
}
