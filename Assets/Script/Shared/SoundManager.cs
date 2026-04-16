using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private int _sfxPoolSize = 8;
    [System.Serializable]
    public struct BgmData
    {
        public EBgmType type;
        public AudioClip clip;
    }
    [System.Serializable]
    public struct SfxData
    {
        public ESfxType type;
        public AudioClip clip;
    }
    [SerializeField] private BgmData[] _bgmList;
    [SerializeField] private SfxData[] _sfxList;
    
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("옵션")]
    [SerializeField] private bool _randomPitch = true;
    [SerializeField] private Vector2 _pitchRange = new Vector2(0.95f, 1.05f);


    private Dictionary<EBgmType, AudioClip> _bgmDict;
    private Dictionary<ESfxType, AudioClip> _sfxDict;
    private List<AudioSource> _sfxPool;

    // 실행안하고 제대로 사운드가 들어갔는지 체크하는 코드(체크할때만 주석을 해제할것)
    /*
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            return;
        }
        BuildMaps();
    }*/

    private void Awake()
    {
        if (_mixer == null)
        {
            CPrint.Error("SoundManager에 AudioMixer안넣었음");
            enabled = false;
            return;
        }
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        BuildMaps();
        InitSfxPool();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void InitSfxPool()
    {
        _sfxPool = new List<AudioSource>();

        for (int i = 0; i< _sfxPoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _sfxSource.outputAudioMixerGroup;
            source.playOnAwake = false;
            source.loop = false;
            _sfxPool.Add(source);
        }
    }

    public void MasterVolumeChange(float value)
    {
        if (_mixer == null)
        {
            return;
        }
        value = Mathf.Clamp(value, 0.001f, 1f);
        _mixer.SetFloat("Master", Mathf.Log10(value) * 20);
        CPrint.Log($"{value}");
    }

    public void BGMVolumeChange(float value)
    {
        if (_mixer == null)
        {
            return;
        }
        value = Mathf.Clamp(value, 0.001f, 1f);
        _mixer.SetFloat("BGM", Mathf.Log10(value) * 20);
        CPrint.Log($"{value}");
    }

    public void SfxVolumeChange(float value)
    {
        if (_mixer == null)
        {
            return;
        }
        value = Mathf.Clamp(value, 0.001f, 1f);
        _mixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        CPrint.Log($"{value}");
    }

    private void BuildMaps()
    {
        _bgmDict = new Dictionary<EBgmType, AudioClip>();
        _sfxDict = new Dictionary<ESfxType, AudioClip>();

        foreach (var data in _bgmList)
        {
            if (data.clip == null)
            {
                CPrint.Warn($"클립 없음 : {data.type}");
                continue;
            }
            if (_bgmDict.ContainsKey(data.type))
            {
                CPrint.Warn($"중복 타입 : {data.type}");
                continue;
            }
            _bgmDict.Add(data.type, data.clip);
        }

        foreach (var data in _sfxList)
        {
            if (data.clip == null)
            {
                CPrint.Warn($"클립 없음 : {data.type}");
                continue;
            }
            if (_sfxDict.ContainsKey(data.type))
            {
                CPrint.Warn($"중복 타입 : {data.type}");
                continue;
            }
            _sfxDict.Add(data.type, data.clip);
        }

        foreach (EBgmType type in System.Enum.GetValues(typeof(EBgmType)))
        {
            if (!_bgmDict.ContainsKey(type))
            {
                CPrint.Warn($"BGM 매핑 안됨 : {type}");
            }
        }

        foreach (ESfxType type in System.Enum.GetValues(typeof(ESfxType)))
        {
            if (!_sfxDict.ContainsKey(type))
            {
                CPrint.Warn($"SFX 매핑 안됨 : {type}");
            }
        }
    }

    public void PlayBgm(EBgmType type)
    {
        if (_bgmDict == null)
        {
            return;
        }
        if (!_bgmDict.TryGetValue(type, out AudioClip clip))
        {
            CPrint.Warn($"Bgm 없음 : {type}");
            return;
        }
        // 중복 재생 방지
        if (_bgmSource.clip == clip)
        {
            return;
        }
        
        _bgmSource.clip = clip;
        _bgmSource.loop = true;
        _bgmSource.Play();
    }

    public void PlaySfx(ESfxType type)
    {
        if (_sfxDict == null)
        {
            return;
        }
        if (!_sfxDict.TryGetValue(type, out AudioClip clip))
        {
            CPrint.Warn($"Sfx 없음 : {type}");
            return;
        }

        AudioSource source = GetSfxSource();
        ApplyPitch(source);
        source.PlayOneShot(clip);
    }

    void ApplyPitch(AudioSource source)
    {
        source.pitch = _randomPitch ? Random.Range(_pitchRange.x, _pitchRange.y) : 1.0f;
    }

    AudioSource GetSfxSource()
    {
        if (_sfxPool == null || _sfxPool.Count == 0)
        {
            return _sfxSource;
        }

        foreach (AudioSource source in _sfxPool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return _sfxPool[0];
    }
}
