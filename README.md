 # Audio Object Library v1.0

## This library implements audio for the Unity engine.

Opportunities:

Creating audio objects in the game space that are empty. You can set the lifespan for these objects in the scene, as well as specify in which vector the sound will be located (XYZ) and the sound file itself. This solution is great for implementing 2D and 3D games. You can apply a distance 3D effect to an Audio Object that will be less audible at a farther distance and vice versa. You can manually destroy the Audio Object.



There is a Music Player in which you can implement a playlist of musical compositions. You can set the sound to fade at the beginning of a new song or turn it off completely. You can set your own playlist for the music player, as well as repeat.

``` C#
musicPlayer.GetVolumeFX();
```
#### Return float value of fx volume
``` C#
musicPlayer.GetVolumeMusic();
```
#### Return float value of music volume

AudioData is responsible for game data on volume as well as on / off sounds and music.

Audio Data Manager controls Audio Data.

### Code example create Audio Object
``` C#
AudioDataManager.CreateAudioObject(position, AudioClip);
```
#### this code return `AudioObject` reference.
Audio Type describes the types of sounds.

``` C#
public enum AudioType
{
    FX,
    Music,
}
```

Demo is included in the repository.

![](https://raw.githubusercontent.com/Siphoin/AudioObjectLibrary/main/demo_screen.png)

## Import:

You can import the library in 2 ways:

unitypackage with demo scene

unitypackage with some library files and resources.
