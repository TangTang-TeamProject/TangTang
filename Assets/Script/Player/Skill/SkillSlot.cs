using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private Player _player;

    public struct SPlayerSkill
    {
        public string id;
        public int level;
    }
    public struct SPlayerArtifact
    {
        public string id;
        public int level;
    }
    public SPlayerSkill[] _playerSkills;
    public SPlayerArtifact[] _playerArtifacts;

    private void Awake()
    {
        if (_player == null)
        {
            CPrint.Error("SkillSlot에 플레이어 없음");
            enabled = false;
            return;
        }
    }

    public void SkillUp(string id, int level)
    {

    }

    public void ArtifackUp(string id, int level)
    {

    }
}
