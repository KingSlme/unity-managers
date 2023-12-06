using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{       
    private AudioSource _musicSourceA;
    private AudioSource _musicSourceB;
    private AudioSource _sfxSource;
    private Coroutine _fadeInCoroutine;
    private Coroutine _fadeOutCoroutine;
    private float _fadeDuration = 2.0f; // Duration for fading in and fading out 
    
    protected override void Awake() 
    {   
        base.Awake();
        if (_musicSourceA == null)
        {
            _musicSourceA = gameObject.AddComponent<AudioSource>();
            _musicSourceA.loop = true;
        }
        if (_musicSourceB == null)
        {
            _musicSourceB = gameObject.AddComponent<AudioSource>();
            _musicSourceB.loop = true;
        }
        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayMusic(MusicEnum musicEnum, float volume = 1.0f) 
    {   
        // _musicSourceA is treated as the primary Audio Source
        _musicSourceB.clip = null;
        StopMusic();
        _musicSourceA.volume = volume;
        _musicSourceA.clip = AssetManager.Instance.GetMusicAudioClip(musicEnum);
        _musicSourceA.Play();
    }

    public void StopMusic()
    {   
        _musicSourceA.Stop();
        _musicSourceB.Stop();
    }

    public void ResumeMusic()
    {
        if (_musicSourceA.clip != null)
        {
            _musicSourceA.UnPause();
        }
        if (_musicSourceB.clip != null)
        {
            _musicSourceB.UnPause();
        }
    }

    public void TransitionMusic(MusicEnum musicEnum, float volume = 1.0f)
    {   
        if (_fadeInCoroutine != null)
        {
            StopCoroutine(_fadeInCoroutine);
        }
        if (_fadeOutCoroutine != null)
        {   
            StopCoroutine(_fadeOutCoroutine);
        }

        if (_musicSourceA.isPlaying)
        {   
            _fadeOutCoroutine = StartCoroutine(FadeOut(_musicSourceA, volume));
            _fadeInCoroutine = StartCoroutine(FadeIn(_musicSourceB, musicEnum, volume));
        }
        else if (_musicSourceB.isPlaying)
        {
            _fadeOutCoroutine = StartCoroutine(FadeOut(_musicSourceB, volume));
            _fadeInCoroutine = StartCoroutine(FadeIn(_musicSourceA, musicEnum, volume));
        }
        else
        {
            _fadeInCoroutine = StartCoroutine(FadeIn(_musicSourceA, musicEnum, volume));
        }
    }

    private IEnumerator FadeIn(AudioSource musicSource, MusicEnum musicEnum, float volume)
    {   
        musicSource.volume = 0;
        musicSource.clip = AssetManager.Instance.GetMusicAudioClip(musicEnum);
        musicSource.Play();
        float elapsedTime = 0.0f;
        while (elapsedTime < _fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(0, volume, elapsedTime / _fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = volume;
    }

    private IEnumerator FadeOut(AudioSource musicSource, float volume)
    {   
        musicSource.volume = volume;
        float elapsedTime = 0.0f;
        while (elapsedTime < _fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(volume, 0, elapsedTime / _fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = 0;
        musicSource.Stop();
        musicSource.clip = null;
    }

    public void PlaySFX(SFXEnum sfxEnum, float volume = 1.0f) => _sfxSource.PlayOneShot(AssetManager.Instance.GetSFXAudioClip(sfxEnum), volume);

    public void PlaySFXAtPoint(SFXEnum sfxEnum, Vector3 position, float volume = 1.0f, float minDistance = 1.0f, float maxDistance = 500.0f)
    {
        GameObject tempObject = CreateSpatialAudioObject(position, volume, minDistance, maxDistance);
        AudioSource audioSource = tempObject.GetComponent<AudioSource>();
        audioSource.clip = AssetManager.Instance.GetSFXAudioClip(sfxEnum);
        audioSource.Play();
        Destroy(tempObject, audioSource.clip.length);
    }

    public GameObject CreateMusicSource(MusicEnum musicEnum, Vector3 position, float volume = 1.0f, float minDistance = 1.0f, float maxDistance = 500.0f)
    {
        GameObject audioSourceObject = CreateSpatialAudioObject(position, volume, minDistance, maxDistance);
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = AssetManager.Instance.GetMusicAudioClip(musicEnum);
        audioSource.Play();
        return audioSourceObject;
    }

    private GameObject CreateSpatialAudioObject(Vector3 position, float volume = 1.0f, float minDistance = 1.0f, float maxDistance = 500.0f)
    {
        GameObject audioSourceObject = new GameObject("3D Spatialization Audio");
        audioSourceObject.transform.position = position;
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        return audioSourceObject;
    }

    public SFXEnum GetRandomSFX(params SFXEnum[] sfxEnums) => sfxEnums[Random.Range(0, sfxEnums.Length)];

    public void ChangeMasterVolume(float value) => AudioListener.volume = value;
    public void ToggleSFX() => _sfxSource.mute = !_sfxSource.mute;
    public void ToggleMusic()
    {
        _musicSourceA.mute = !_musicSourceA.mute;
        _musicSourceB.mute = !_musicSourceB.mute;
    } 
}