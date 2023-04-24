using Siphoin.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AudioObjectLib
{
    public class AudioDataManager : MonoBehaviour
    {
    private static AudioDataManager _manager;

        private AudioData _audioData;

    private PoolMono<AudioObject> _pool;

    [SerializeField, Min(2)] private int _poolCount = 15;


    public event Action<float> OnFXVolumeChanged;

    public event Action<float> OnMusicVolumeChanged;

    public event Action<bool> OnMusicEnabled;


    private const string PATH_PREFAB_AUDIO_OBJECT = "Prefabs/Audio/AudioObject";

    private AudioObject _audioObjectPrefab;

    public static AudioDataManager Manager { get => _manager; }

   private void Awake()
    {
        if (_manager is null)
        {
            _manager = this;

            _audioObjectPrefab = Resources.Load<AudioObject>(PATH_PREFAB_AUDIO_OBJECT);

            if (_audioObjectPrefab is null)
            {
                throw new AudioDataManagerException("audio object prefab not found");
            }

            _audioData = new AudioData();

            Transform rootPool = new GameObject("Pool").transform;

            rootPool.SetParent(transform);

            _pool = new PoolMono<AudioObject>(_audioObjectPrefab, rootPool, _poolCount, true);



            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

    }


    public float GetVolumeFX ()
    {
        return _audioData.FXVolume;
    }

    public float GetVolumeMusic ()
    {
        return _audioData.MusicVolume;
    }

    public bool GetMusicEnabled ()
    {
        return _audioData.MusicEnabled;
    }

    public void SetVolumeFX (float value)
    {
        _audioData.FXVolume = ClampingVolume(value);

        OnFXVolumeChanged?.Invoke(_audioData.FXVolume);
    }

    public void SetVolumeMusic (float value)
    {
        _audioData.MusicVolume = ClampingVolume(value);

        OnMusicVolumeChanged?.Invoke(_audioData.MusicVolume);
    }

    private float ClampingVolume (float value)
    {
        return Mathf.Clamp(value, 0.0f, 1.0f);
    }

    public void SetEnabledMusic (bool status)
    {
        _audioData.MusicEnabled = status;

        OnMusicEnabled?.Invoke(_audioData.MusicEnabled);
    }

    public AudioObject CreateAudioObject (Vector3 position, AudioClip clip)
    {
            AudioObject audioObject = null;

            /* Legacy
        audioObject = Instantiate(_audioObjectPrefab);

            */

            audioObject = _pool.GetFreeElement();

            audioObject.transform.position = position;

            audioObject.GetAudioSource().clip = clip;

            return audioObject;
    }


}
}
