using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private SkillSpawner _spawner;
    [SerializeField] private int _maxSkillNum = 4;
    [SerializeField] private int _maxArtifactNum = 4;

    [Serializable]
    public class PlayerSkill
    {
        public string id;
        public int level;
    }
    [Serializable]
    public class PlayerArtifact
    {
        public string id;
        public int level;
    }
    [SerializeField] private List<PlayerSkill> _playerSkills = new List<PlayerSkill>();
    [SerializeField] private List<PlayerArtifact> _playerArtifacts = new List<PlayerArtifact>();
    private readonly Dictionary<string, PlayerSkill> _skillDict = new Dictionary<string, PlayerSkill>();
    private readonly Dictionary<string, PlayerArtifact> _artifactDict = new Dictionary<string, PlayerArtifact>();

    public int SkillNum => _playerSkills.Count;
    public int ArtifactNum => _playerArtifacts.Count;

    private void Awake()
    {
        if (_player == null)
        {
            CPrint.Error("SkillSlot에 플레이어 없음");
            enabled = false;
            return;
        }
        if (_spawner == null)
        {
            CPrint.Error("SkillSlot에 스킬스포너 없음");
            enabled = false;
            return;
        }
    }

    public void SkillLevel(int num, out string id, out int level)
    {
        if (num >= _playerSkills.Count)
        {
            id = null;
            level = 0;
            return;
        }
        id = _playerSkills[num].id;
        level = _playerSkills[num].level;
    }

    public bool IsSkillFull()
    {
        return _playerSkills.Count >= _maxSkillNum;
    }

    public void ArtifactLevel(int num, out string id, out int level)
    {
        if (num >= _playerArtifacts.Count)
        {
            id = null;
            level = 0;
            return;
        }
        id = _playerArtifacts[num].id;
        level = _playerArtifacts[num].level;        
    }

    public bool IsArtifactFull()
    {
        return _playerArtifacts.Count >= _maxArtifactNum;
    }

    public void SkillUp(string id)
    {
        int level = 1;
        if (_skillDict.TryGetValue(id, out PlayerSkill skill))
        {
            skill.level++;
            level = skill.level;
        }
        else
        {
            if (_playerSkills.Count >= _maxSkillNum)
            {
                CPrint.Warn("스킬 슬롯 꽉 참");
                return;
            }
            PlayerSkill newSkill = new PlayerSkill
            {
                id = id,
                level = level
            };
            _playerSkills.Add(newSkill);
            _skillDict.Add(id, newSkill);
        }
        _spawner.GetSkill(id, level);
    }

    public void ArtifactUp(string id)
    {
        int level = 1;
        if (_artifactDict.TryGetValue(id, out PlayerArtifact artifact))
        {
            artifact.level++;
            level = artifact.level;
        }
        else
        {
            if (_playerArtifacts.Count >= _maxArtifactNum)
            {
                CPrint.Warn("아티팩트 슬롯 꽉참");
                return;
            }
            PlayerArtifact newArtifact = new PlayerArtifact
            {
                id = id,
                level = level
            };
            _playerArtifacts.Add(newArtifact);
            _artifactDict.Add(id, newArtifact);
        }
        _player.GetArtifact(id, level);
    }
}