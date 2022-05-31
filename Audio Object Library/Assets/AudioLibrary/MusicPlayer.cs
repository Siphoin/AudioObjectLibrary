using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace AudioObjectLib
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
    private const float TIME_OUT_NEW_TRACK = 4f;

    private bool _isPlaying = true;

    private int _indexSelectedTrack = 0;



    public event UnityAction<bool> OnStateMusicPlayer;

    public event UnityAction<string> OnNewTrackPlaying;

    private delegate void NewState();

    private NewState _statePlay;

    private NewState _stateStop;

    private NewState _statePause;

    [SerializeField] private AudioClip[] _musicList;

     private AudioClip[] _musicListCached;

    private AudioSource _audioSource;

    private AudioDataManager _audioManager;

   [SerializeField] private bool _lerping = false;

    private AudioClip _lastAudioClip;

    private AudioClip _selectedTrack;

    public bool IsPlaying => _isPlaying;

    public string NameCurrentPlayingClip => _selectedTrack.name;


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

            _audioManager.OnMusicVolumeChanged += ChangeVolume;
            _audioManager.OnMusicEnabled += SetStatusMusic;

            _musicListCached = GetClipsWithArrayClips(_musicListCached, _musicList);

            StartCoroutine(WaitNewTrack());

            InitStates();

        }

        private void InitStates()
        {
            _statePlay += _audioSource.Play;

            _statePlay += SetNewState;

            _stateStop += _audioSource.Stop;

            _stateStop += SetNewState;

            _statePause += _audioSource.Pause;

            _statePause += SetNewState;
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

        if (_musicList.Length == 1)
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
        if (_lerping) 
        {
        StartCoroutine(LerpingVolume());            
        }

        else 
        {
            ChangeVolume(_audioManager.GetVolumeMusic());
        }

        PlayTrack(_selectedTrack);
    }

    private void SelectRansomTrack()
    {
        if (_musicList.Length > 1)
        {
        AudioClip[] tracks = _musicList.Where(track => track != _lastAudioClip).ToArray();
        _indexSelectedTrack = Random.Range(0, tracks.Length);

        _selectedTrack = tracks[_indexSelectedTrack];
        }

        else
        {
            SetZeroIndexMusic();
        }

    }



    private IEnumerator LerpingVolume()
    {
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

        OnNewTrackPlaying?.Invoke(track.name);
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

        if (_lerping) 
        {
            StartCoroutine(LerpingVolume());
        }

        else {
            ChangeVolume(_audioManager.GetVolumeMusic());
        }
        

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

    private void SetNewState () 
    {
         _isPlaying = !_isPlaying;

         OnStateMusicPlayer?.Invoke(_isPlaying);
    }

    public void NextTrack ()
        {

            if (!IsPlaying)
            {
                return;
            }

            SetZeroIndexMusic();

            if (_indexSelectedTrack >= _musicList.Length - 1)
            {
                _indexSelectedTrack--;
            }

            PlayTrack(_musicList[_indexSelectedTrack]);

            ReloadWaitNextTrack();
        }

        public void BackTrack ()
        {

            if (!IsPlaying) 
            {
                return;
            }
            SetZeroIndexMusic();
           
            if (_indexSelectedTrack < _musicList.Length - 1) 
            {
                  _indexSelectedTrack++;
            }

            PlayTrack(_musicList[_indexSelectedTrack]);

            ReloadWaitNextTrack();

        }

        private void ReloadWaitNextTrack()
        {
            StopAllCoroutines();

            StartCoroutine(WaitNewTrack());
        }

        private void SetZeroIndexMusic()
        {
            if (_musicList.Length == 0)
            {
                _selectedTrack = _musicList[0];
            }
        }

        private void OnDestroy() 
        {
            _audioManager.OnMusicVolumeChanged += ChangeVolume;
            _audioManager.OnMusicEnabled += SetStatusMusic;
        }
    private void ChangeVolume (float value) => _audioSource.volume = value;

    public void Play () => _statePlay();

    public void Stop () => _stateStop();

    public void Pause () => _statePause();




}
}
