using UnityEngine;
using AudioObjectLib;
using UnityEngine.UI;
using System;
namespace AudioObjectLib.Demo
{
    public class ControllerCreatingDemoAudioObject : MonoBehaviour {
    [SerializeField] private InputField _inputX;
    [SerializeField] private InputField _inputY;
    [SerializeField] private InputField _inputZ;

    [SerializeField] private AudioClip _testClip;

    [SerializeField] private Button _buttonCreate;

    private AudioDataManager _audioManager;

    private void Start() {

        _audioManager = GameObject.FindObjectOfType<AudioDataManager>();

        if (!_audioManager)
        {
            throw new NullReferenceException("audio manager not found");
        }

        if (_inputY == null) 
        {
           throw new NullReferenceException("input y not seted!");
        }

        if (_inputX == null) 
        {
           throw new NullReferenceException("input x not seted!");
        }

        if (_inputZ == null) 
        {
           throw new NullReferenceException("input z not seted!");
        }

        if (_testClip == null) 
        {
            throw new NullReferenceException("test clip not seted!");
        }

        if (_buttonCreate == null) 
        {
            throw new NullReferenceException("button create not seted!");
        }

        _buttonCreate.onClick.AddListener(CreateAudioObject);


    }

    private void CreateAudioObject () 
    {
        float x = float.Parse(_inputX.text);
        float y = float.Parse(_inputY.text);
        float z = float.Parse(_inputZ.text);

        Vector3 position = new Vector3(x, y, z);

        AudioObject audioObject = _audioManager.CreateAudioObject(position, _testClip);

        audioObject.GetAudioSource().Play();
        audioObject.RemoveIFNotPlaying();


    }


}
}
