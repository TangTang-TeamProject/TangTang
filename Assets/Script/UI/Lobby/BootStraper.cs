using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootStraper : MonoBehaviour
{
    readonly RateSetting rs = new RateSetting();

    [SerializeField]
    private ArtifactRegistry artifact;
    [SerializeField]
    private EnemyRegistry enemy;
    [SerializeField]
    private EquipRegistry equip;
    [SerializeField]
    private ItemRegistry item;
    [SerializeField]
    private PlayerRegistry player;
    [SerializeField]
    private WaveRegistry wave;
    [SerializeField]
    private EvolutionRegistry evo;
    [SerializeField]
    private SkillRegistry skill;
    [SerializeField]
    private SkillLevelRegistry level;



    private void Awake()
    {
        SaveManager.Load();

        SO_Set();

        SetScreen();
    }

    private void Start()
    {
        SetSound();
    }

    void SetScreen()
    {
        Screen.SetResolution(
        rs.dropDownMap[SaveManager.data.rateIndex].x,
        rs.dropDownMap[SaveManager.data.rateIndex].y,
        SaveManager.data.fullScreen);
    }

    void SetSound()
    {
        SoundManager.Instance.MasterVolumeChange(SaveManager.data.masterVolume);
        SoundManager.Instance.BGMVolumeChange(SaveManager.data.bgmVolume);
        SoundManager.Instance.SfxVolumeChange(SaveManager.data.sfxVolume);
    }

    void SO_Set()
    {
        artifact.MakeDic();
        enemy.MakeDic();
        equip.MakeDic();
        item.MakeDic();
        player.MakeDic();
        wave.MakeDic();
        evo.MakeDic();
        skill.MakeDic();
        level.MakeDic();
    }
}
