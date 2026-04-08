using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // 미완 오디오믹서로 사용 필요 사운드 저장도 SO로 저장할지 검토중
    public static SoundManager Instance { get; private set; }

    public enum EBgmType
    {
        Lobby,
        InGame,
    }

    public enum ESfxType
    {
        Spear,
        Trident,
    }
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioClip[] _bgmClips;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip[] _sfxClips;

    [Header("옵션")]
    [SerializeField, Range(0f, 1f)] private float _masterVolume = 1.0f;
    [SerializeField, Range(0f, 1f)] private float _bgmVolume = 1.0f;
    [SerializeField, Range(0f, 1f)] private float _sfxVolume = 1.0f;
    [SerializeField] private bool _randomPitch = true;
    [SerializeField] private Vector2 _pitchRange = new Vector2(0.95f, 1.05f);


    private readonly Dictionary<string, int> _nameToId = new Dictionary<string, int>();

    public float MasterVolume => _masterVolume;
    public float BGMVolume => _bgmVolume;
    public float SfxVolume => _sfxVolume;

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
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // 시작시 브금0번 재생 시험용 나중에 재구축예정
        if (_bgmClips == null || _bgmSource == null)
        {
            return;
        }
        _bgmSource.loop = true;
        _bgmSource.clip = _bgmClips[0];
        _bgmSource.Play();
    }

    public void MasterVolumeChange(float value)
    {
        if (_mixer == null)
        {
            return;
        }
        ValueCheck(value);
        _mixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }

    public void BGMVolumeChange(float value)
    {
        if (_mixer == null)
        {
            return;
        }
        ValueCheck(value);
        _mixer.SetFloat("BGM", Mathf.Log10(value) * 20);
    }

    public void SfxVolumeChange(float value)
    {
        if (_mixer == null)
        {
            return;
        }
        ValueCheck(value);
        _mixer.SetFloat("SFX", Mathf.Log10(value) * 20);
    }

    float ValueCheck(float value)
    {
        if (value <= 0.001f)
        {
            value = 0.001f;
        }
        return value;
    }

    private void BuildMaps()
    {
        _nameToId.Clear();
        if (_sfxClips == null)
        {
            return;
        }

        for (int i = 0; i < _sfxClips.Length; i++)
        {
            AudioClip ac = _sfxClips[i];
            if (ac == null)
            {
                continue;
            }

            if (_nameToId.ContainsKey(ac.name))
            {
                Debug.LogWarning($"이름 중복 : {ac.name}");
                continue;
            }

            _nameToId.Add(ac.name, i);
        }
    }

    private bool TryGetSfxId(ESfxType type, out int id)
    {
        return _nameToId.TryGetValue(type.ToString(), out id);
    }

    public void PlaySfx(ESfxType type)
    {
        if (_sfxClips == null)
        {
            return;
        }
        if (!TryGetSfxId(type, out int id))
            return;

        ApplyPitch(_sfxSource);
        _sfxSource.PlayOneShot(_sfxClips[id]);
    }

    void ApplyPitch(AudioSource source)
    {
        source.pitch = _randomPitch ? Random.Range(_pitchRange.x, _pitchRange.y) : 1.0f;
    }
}
