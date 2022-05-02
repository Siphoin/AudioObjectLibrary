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
    [SerializeField] private AudioClip[] musicList;

    [SerializeField] private AudioClip[] musicListCached;

    private AudioSource audioSource;

    private AudioDataManager audioManager;

    private bool lerping = false;

    private AudioClip lastAudioClip;

    private AudioClip selectedTrack;
        // Use this for initialization
        void Start()
        {
        if (musicList.Length == 0)
        {
            throw new MusicPlayerException("music list is emtry");
        }
        if (AudioDataManager.Manager == null)
        {
            throw new MusicPlayerException("audio manager not found");
        }
        if (!TryGetComponent(out audioSource))
        {
            throw new MusicPlayerException("music list is emtry");
        }


        audioManager = AudioDataManager.Manager;

        audioManager.OnFXVolumeChanged += ChangeVolume;
        audioManager.OnMusicEnabled += SetStatusMusic;

        musicListCached = GetClipsWithArrayClips(musicListCached, musicList);
        StartCoroutine(WaitNewTrack());



    }

    private void SetStatusMusic(bool enabled)
    {
        if (enabled)
        {
            if (!audioSource.isPlaying)
            {
                NewTrack();
            }
        }

        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void NewTrack()
    {

        if (musicList.Length == 0)
        {
            selectedTrack = musicList[0];
        }
        else
        {
            SelectRansomTrack();

        }

        if (!audioManager.GetMusicEnabled())
        {
            return;
        }

        StartCoroutine(LerpingVolume());
        PlayTrack(selectedTrack);
    }

    private void SelectRansomTrack()
    {
        if (musicList.Length > 1)
        {
        AudioClip[] tracks = musicList.Where(track => track != lastAudioClip).ToArray();
        
        selectedTrack = tracks[Random.Range(0, tracks.Length)];
        }

        else
        {
            selectedTrack = musicList[0];
        }

    }


    private void ChangeVolume (float value)
    {
        if (!lerping)
        {
            audioSource.volume = value;
        }
    }

    private IEnumerator LerpingVolume()
    {
        lerping = true;
        float lerpValue = 0;
        while (true)
        {
            float fpsRate = 1.0f / 60.0f;

            yield return new WaitForSeconds(fpsRate);
            lerpValue += fpsRate;
            audioSource.volume = Mathf.Lerp(0, audioManager.GetVolumeMusic(), lerpValue);

            if (lerpValue >= 1)
            {
                lerping = false;
                yield break;
            }
        }
    }

    private IEnumerator WaitNewTrack ()
    {
        NewTrack();


        while (true)
        {
        yield return new WaitForSecondsRealtime(selectedTrack.length + TIME_OUT_NEW_TRACK);
            if (audioManager.GetMusicEnabled())
            {
             NewTrack();
            }

        }

        
    }

    private void PlayTrack (AudioClip track)
    {

        audioSource.clip = track;
        lastAudioClip = track;
        audioSource.Play();
    }

    public void ReplaceTrack (AudioClip track)
    {
        if (track == null)
        {
            throw new MusicPlayerException("track is null");
        }
        StopAllCoroutines();


        audioSource.Stop();

        musicList = new AudioClip[1];

        musicList[0] = track;

        StartCoroutine(LerpingVolume());

        StartCoroutine(WaitNewTrack());

    }

    public void ReturnOriginalListMusic ()
    {

        musicList = GetClipsWithArrayClips(musicList, musicListCached);

        StopAllCoroutines();

        audioSource.Stop();
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
        AudioClip[] clips = new AudioClip[musicList.Length];
        musicList.CopyTo(clips, 0);
        return clips;
    }

    public void SetTrackList (AudioClip[] tracks)
    {
        StopAllCoroutines();
        musicList = GetClipsWithArrayClips(musicList, tracks);
        StartCoroutine(WaitNewTrack());
    }


}
}
