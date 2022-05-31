 # Audio Object Library v1.3

## This library implements audio for the Unity engine.

Opportunities:
## Audio Object
Creating audio objects in the game space that are empty. You can set the lifespan for these objects in the scene, as well as specify in which vector the sound will be located (XYZ) and the sound file itself. This solution is great for implementing 2D and 3D games. You can apply a distance 3D effect to an Audio Object that will be less audible at a farther distance and vice versa. You can manually destroy the Audio Object.

``` C#
audioObject.RemoveIFNotPlaying();
```

#### Seted lifespan of AudioObject (lifespan equals of length audio file)

``` C#
private void ChangeVolume (float value)
```
#### Changing volume of AudioObject

## Music Player
There is a Music Player in which you can implement a playlist of musical compositions. You can set the sound to fade at the beginning of a new song or turn it off completely. You can set your own playlist for the music player, as well as repeat.

![](https://raw.githubusercontent.com/Siphoin/AudioObjectLibrary/main/musicPlayer_screen.png)

``` C#
musicPlayer.GetVolumeFX();
```
#### Return float value of fx volume
``` C#
musicPlayer.GetVolumeMusic();
```

``` C#
musicPlayer.Pause();
```
#### Paused current music

``` C#
musicPlayer.Stop();
```
#### Stopped current music

``` C#
musicPlayer.Play();
```
#### Return playing state of music player

## Audio Data
AudioData is responsible for game data on volume as well as on / off sounds and music.

## Audio Data Manager
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

unitypackage with demo scene. (AudioObjectLibrary_Demo.unitypackage)

unitypackage with some library files and resources. (AudioObjectLibrary_Lib.unitypackage)

Please add a prefab Audio Object to Assets/Resources (emtry object with script AudioObject!!!!
