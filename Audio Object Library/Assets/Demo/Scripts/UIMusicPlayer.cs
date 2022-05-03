using UnityEngine;
using System;
using UnityEngine.UI;
using AudioObjectLib;

namespace AudioObjectLib.Demo
{
    public class UIMusicPlayer : MonoBehaviour {

    [SerializeField] private Sprite _stopSprite;

    [SerializeField] private Sprite _playSprite;

    [SerializeField] private Image _iconStatusPlaying;

    [SerializeField] private MusicPlayer _musicPlayer;

    [SerializeField] private Button _buttonStatusPlayer;

    [SerializeField] private Button _buttonNextMusic;

    [SerializeField] private Button _buttonBackMusic;
    
    private void Start() {

        if (!_playSprite)
        {
            throw new NullReferenceException("play sprite not seted");
        }

        if (!_stopSprite)
        {
            throw new NullReferenceException("stop sprite not seted");
        }

        if (!_iconStatusPlaying)
        {
            throw new NullReferenceException("icon status playing not seted");
        }

        if (!_musicPlayer)
        {
            throw new NullReferenceException("music player not seted");
        }

        if (!_buttonStatusPlayer)
        {
            throw new NullReferenceException("button status player not seted");
        }

         if (!_buttonStatusPlayer)
        {
            throw new NullReferenceException("button status player not seted");
        }

        if (!_buttonNextMusic)
        {
            throw new NullReferenceException("button next music not seted");
        }

        if (!_buttonBackMusic)
        {
            throw new NullReferenceException("button back music not seted");
        }

        _musicPlayer.OnStateMusicPlayer += SetNewIconStatus;

        _buttonStatusPlayer.onClick.AddListener(SetStatusMusicPlayer);

        _buttonNextMusic.onClick.AddListener(NextMusic);

        _buttonBackMusic.onClick.AddListener(BackMusic);





    }

    private void SetStatusMusicPlayer() {
        if (_musicPlayer.IsPlaying)
        {
            _musicPlayer.Pause();
        }

        else
        {
            _musicPlayer.Play();
        }
    }

    private void SetNewIconStatus (bool status) => _iconStatusPlaying.sprite = status ? _playSprite : _stopSprite;

    private void NextMusic () => _musicPlayer.NextTrack();

    private void BackMusic () => _musicPlayer.BackTrack();

    private void OnDestroy() => _musicPlayer.OnStateMusicPlayer -= SetNewIconStatus;
}
}
