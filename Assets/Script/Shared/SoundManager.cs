using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        BuildMaps();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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
