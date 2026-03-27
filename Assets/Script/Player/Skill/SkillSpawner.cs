using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    // SO에서 파라미터를 받고 갯수에 맞춰서 풀에서 InitCreatePool호출 생산품의 Init에 파라미터 삽입
    [SerializeField] private SkillPool _pool;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _spawnDir;

    private WaitForSeconds _spearRate= new WaitForSeconds(2.2f);
    private Coroutine _spearCo;

    private void Awake()
    {
        if (_pool == null)
        {
            CPrint.Log("스킬스포너에 풀 참조 안됐음");
            enabled = false;
            return;
        }

        if (_player == null)
        {
            CPrint.Log("스킬스포너에 플레이어 참조 안됐음");
            return;
        }

        if (_spawnDir == null)
        {
            CPrint.Log("스킬스포너에 공격방향 참조 안됐음");
            return;
        }

    }

    void Start()
    {
        GetSkill("Spear", 5);
        GetSkill("Trident", 5);
        GetSkill("Mace", 5);

        _spearCo = StartCoroutine(Co_SpearFire());
    }

    IEnumerator Co_SpearFire()
    {
        while(_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack spear = _pool.UseSkill("Spear");
            SkillAttack trident = _pool.UseSkill("Trident");
            SkillAttack mace = _pool.UseSkill("Mace");

            spear.transform.position = _player.transform.position;
            spear.transform.rotation = Quaternion.Euler(0, 0, 90f);
            spear.Init(1, 1, 4, _pool);
            spear.gameObject.SetActive(true);

            trident.transform.position = _player.transform.position;
            trident.transform.rotation = _spawnDir.rotation;
            trident.Init(1, 1, 4, _pool);
            trident.gameObject.SetActive(true);

            mace.transform.position = _player.transform.position;
            mace.Init(1, 1, 4, _pool);
            mace.gameObject.SetActive(true);


            yield return _spearRate;
        }
    }

    private void OnDisable()
    {
        if (_spearCo != null)
        {
            StopCoroutine(_spearCo);
            _spearCo = null;
        }
    }

    // 나중에 플레이어 같은데서 실행받고 SO같은걸 넘긴다
    public void GetSkill(string tag, int num)
    {
        _pool.InitCreateSkill(tag, num);
    }
}
