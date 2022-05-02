using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioObjectLib
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
    private const float TIME_OUT_NEW_TRACK = 4f;
    [SerializeField] private AudioClip[] _musicList;

    [SerializeField] private AudioClip[] _musicListCached;

    private AudioSource _audioSource;

    private AudioDataManager _audioManager;

    private bool _lerping = false;

    private AudioClip _lastAudioClip;

    private AudioClip _selectedTrack;
      private void Start()
        {
        if (_musicList.Length == 0)
        {
            throw new MusicPlayerException("music list is emtry");
        }
        if (AudioDataManager.Manager == null)
        {
            throw new MusicPlayerException("audio manager not found");
        }
        if (!TryGetComponent(out _audioSource))
        {
            throw new MusicPlayerException("music list is emtry");
        }


        _audioManager = AudioDataManager.Manager;

        _audioManager.OnFXVolumeChanged += ChangeVolume;
        _audioManager.OnMusicEnabled += SetStatusMusic;

        _musicListCached = GetClipsWithArrayClips(_musicListCached, _musicList);

        StartCoroutine(WaitNewTrack());



    }

    private void SetStatusMusic(bool enabled)
    {
        if (enabled)
        {
            if (!_audioSource.isPlaying)
            {
                NewTrack();
            }
        }

        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }

    private void NewTrack()
    {

        if (_musicList.Length == 0)
        {
            _selectedTrack = _musicList[0];
        }
        else
        {
            SelectRansomTrack();

        }

        if (!_audioManager.GetMusicEnabled())
        {
            return;
        }

        StartCoroutine(LerpingVolume());
        PlayTrack(_selectedTrack);
    }

    private void SelectRansomTrack()
    {
        if (_musicList.Length > 1)
        {
        AudioClip[] tracks = _musicList.Where(track => track != _lastAudioClip).ToArray();
        
        _selectedTrack = tracks[Random.Range(0, tracks.Length)];
        }

        else
        {
            _selectedTrack = _musicList[0];
        }

    }


    private void ChangeVolume (float value)
    {
        if (!_lerping)
        {
            _audioSource.volume = value;
        }
    }

    private IEnumerator LerpingVolume()
    {
        _lerping = true;
        float lerpValue = 0;
        while (true)
        {
            float fpsRate = 1.0f / 60.0f;

            yield return new WaitForSeconds(fpsRate);
            lerpValue += fpsRate;
            _audioSource.volume = Mathf.Lerp(0, _audioManager.GetVolumeMusic(), lerpValue);

            if (lerpValue >= 1)
            {
                _lerping = false;
                yield break;
            }
        }
    }

    private IEnumerator WaitNewTrack ()
    {
        NewTrack();


        while (true)
        {
        yield return new WaitForSecondsRealtime(_selectedTrack.length + TIME_OUT_NEW_TRACK);

            if (_audioManager.GetMusicEnabled())
            {
             NewTrack();
            }

        }

        
    }

    private void PlayTrack (AudioClip track)
    {

        _audioSource.clip = track;
        _lastAudioClip = track;
        _audioSource.Play();
    }

    public void ReplaceTrack (AudioClip track)
    {
        if (track == null)
        {
            throw new MusicPlayerException("track is null");
        }
        StopAllCoroutines();


        _audioSource.Stop();

        _musicList = new AudioClip[1];

        _musicList[0] = track;

        StartCoroutine(LerpingVolume());

        StartCoroutine(WaitNewTrack());

    }

    public void ReturnOriginalListMusic ()
    {

        _musicList = GetClipsWithArrayClips(_musicList, _musicListCached);

        StopAllCoroutines();

        _audioSource.Stop();
        StartCoroutine(WaitNewTrack());

    }

    private AudioClip[] GetClipsWithArrayClips (AudioClip[] arrayTarget, AudioClip[] arrayGet)
    {
        arrayTarget = new AudioClip[arrayGet.Length];

        for (int i = 0; i < arrayGet.Length; i++)
        {
           arrayTarget[i] = arrayGet[i];
        }

        return arrayTarget;
    }

    public AudioClip[] GetActiveTrackList ()
    {
        AudioClip[] clips = new AudioClip[_musicList.Length];
        _musicList.CopyTo(clips, 0);
        return clips;
    }

    public void SetTrackList (AudioClip[] tracks)
    {
        StopAllCoroutines();
        _musicList = GetClipsWithArrayClips(_musicList, tracks);
        StartCoroutine(WaitNewTrack());
    }


}
}
