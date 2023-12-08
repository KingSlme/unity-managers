# unity-managers
A group of scripts useful for managing common systems within Unity projects.

## Singleton
*A generic implementation of a useful design pattern that allows for global access and ensurance of only one instance of itself existing.*

## Asset Manager
*Prevents the need of storing assets on MonoBehaviour components. Instead, assets can be stored on an AssetManager prefab within the Resources folder.*

### Methods

#### GetMusicAudioClip
*Returns the AudioClip associated with a Music enum.*
```cs
public AudioClip GetMusicAudioClip(MusicEnum music)
```

#### GetSFXAudioClip
*Returns the AudioClip associated with a SFX enum.*
```cs
public AudioClip GetSFXAudioClip(SFXEnum sfx)
```

#### GetUITexture
*Returns the Texture associated with a UITexture enum.*
```cs
public Texture GetUITexture(UITextureEnum texture)
```

#### GetPrefab
*Returns the GameObject associated with a Prefab enum.*
```cs
public GameObject GetPrefab(PrefabEnum prefab)
```

## Audio Manager
*Allows manipulation of audio without the use of explicitly putting Audio Source components on objects.*

### Methods

#### PlayMusic
*Loops an AudioClip given by a Music enum.*
```cs
public void PlayMusic(MusicEnum musicEnum, float volume = 1.0f)
```

#### StopMusic
*Stops the current music.*
```cs
public void StopMusic()
```

#### ResumeMusic
*Resumes the current music.*
```cs
public void ResumeMusic()
```

#### TransitionMusic
*Crossfade to new AudioClip given by a Music enum.*
```cs
public void TransitionMusic(MusicEnum musicEnum, float volume = 1.0f)
```

#### PlaySFX
*Plays an AudioClip given by a SFX enum.*
```cs
public void PlaySFX(SFXEnum sfxEnum, float volume = 1.0f)
```

#### PlaySFXAtPoint
*Plays an AudioClip given by a SFX enum at a position.*
```cs
public void PlaySFXAtPoint(SFXEnum sfxEnum, Vector3 position, float volume = 1.0f, float minDistance = 1.0f, float maxDistance = 500.0f)
```

#### CreateMusicSource
*Creates and returns a GameObject with a 3D spatial looping AudioSource with an AudioClip given by a Music enum at a position.*
```cs
public GameObject CreateMusicSource(MusicEnum musicEnum, Vector3 position, float volume = 1.0f, float minDistance = 1.0f, float maxDistance = 500.0f)
```

#### GetRandomSFX
*Returns a random SFX enum from a variable number of provided SFX enums.*
```cs
public SFXEnum GetRandomSFX(params SFXEnum[] sfxEnums)
```

#### ChangeMasterVolume
*Changes the volume of the Audio Manager's AudioSources.*
```cs
public void ChangeMasterVolume(float value)
```

#### ToggleSFX
*Toggles the Audio Manager's SFX AudioSource.*
```cs
public void ToggleSFX()
```
#### ToggleMusic
*Toggles the Audio Manager's Music AudioSource.*
```cs
public void ToggleMusic()
```
