using UnityEngine;
using System.Collections.Generic;


// Remember to attach this script to a prefab named AssetManager within the Resources folder
public enum MusicEnum
    {

    }
    public enum SFXEnum
    {

    }
    public enum UITextureEnum
    {

    }
    public enum PrefabEnum
    {

    }

public class AssetManager : Singleton<AssetManager>
{    
    private Dictionary<MusicEnum, AudioClip> _musicDictionary = new Dictionary<MusicEnum, AudioClip>();
    private Dictionary<SFXEnum, AudioClip> _sfxDictionary = new Dictionary<SFXEnum, AudioClip>();
    private Dictionary<UITextureEnum, Texture> _uiTextureDictionary = new Dictionary<UITextureEnum, Texture>();
    private Dictionary<PrefabEnum, GameObject> _prefabDictionary = new Dictionary<PrefabEnum, GameObject>();

    private static AssetManager _instance;

    public new static AssetManager Instance
    {
        get
        {   
            if (_instance == null)
            {   
                _instance = FindObjectOfType<AssetManager>();
                if (_instance == null)
                {
                    _instance = (Instantiate(Resources.Load("AssetManager")) as GameObject).GetComponent<AssetManager>();
                }
            }
            return _instance;
        }
    }
    
    // Assets:
    [Header("Music")] 
    [SerializeField] private MusicAudioClip[] _musicArray;
    [Header("SFX")]
    [SerializeField] private SFXAudioClip[] _sfxArray;
    [Header("Textures")] 
    [SerializeField] private UITexture[] _uiTextureArray;
    [Header("Prefabs")] 
    [SerializeField] private Prefab[] _prefabArray;

    [System.Serializable] public class MusicAudioClip
    {
        public MusicEnum MusicAudioClipMember;
        public AudioClip MusicAudioClipAsset;
    }

    [System.Serializable] public class SFXAudioClip
    {
        public SFXEnum SFXAudioClipMember;
        public AudioClip SFXAudioClipAsset;
    }
    [System.Serializable] public class UITexture
    {
        public UITextureEnum UITextureMember;
        public Texture UITextureAsset;
    }
    [System.Serializable] public class Prefab
    {
        public PrefabEnum PrefabMember;
        public GameObject PrefabAsset;
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeDictionaries(_musicArray, _musicDictionary);
        InitializeDictionaries(_sfxArray, _sfxDictionary);
        InitializeDictionaries(_uiTextureArray, _uiTextureDictionary);
        InitializeDictionaries(_prefabArray, _prefabDictionary);
    }

    private void InitializeDictionaries<T, U, V>(T[] assetArray, Dictionary<U, V> dictionary) where V : class
    {
        foreach (T asset in assetArray)
        {
            var assetMember = (U)asset.GetType().GetField($"{typeof(T).Name}Member").GetValue(asset); 
            var assetField = (V)asset.GetType().GetField($"{typeof(T).Name}Asset").GetValue(asset);
            if (!assetField.Equals(null)) {
                dictionary.Add(assetMember, assetField); 
            }
        }
    }

    private V GetAsset< U, V>(Dictionary<U, V> dictionary, U assetKey) where V : class
    {
        if (dictionary.TryGetValue(assetKey, out V assetValue))
        {
            return assetValue;
        }
        Debug.LogError($"Missing {assetKey} Asset!"); 
        return null;
    }

    public AudioClip GetMusicAudioClip(MusicEnum music) => GetAsset(_musicDictionary, music);  
    public AudioClip GetSFXAudioClip(SFXEnum sfx) => GetAsset(_sfxDictionary, sfx);
    public Texture GetUITexture(UITextureEnum texture) => GetAsset(_uiTextureDictionary, texture);
    public GameObject GetPrefab(PrefabEnum prefab) => GetAsset(_prefabDictionary, prefab);
}