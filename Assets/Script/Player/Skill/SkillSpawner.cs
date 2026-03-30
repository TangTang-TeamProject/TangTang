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
        GetSkill("Axe", 6);

        _spearCo = StartCoroutine(Co_AxeFire());
    }

    // 임시로 만든 로직이므로 SO같은 데이터가 들어오면 수정될 것
    #region 스킬 코루틴
    IEnumerator Co_ArrowFire()
    {
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack arrow = _pool.UseSkill("Arrow");

            arrow.transform.position = transform.position;
            arrow.transform.up = _spawnDir.right;
            arrow.Init(1, 1, 4, _pool);
            arrow.gameObject.SetActive(true);

            yield return _spearRate;
        }
    }

    IEnumerator Co_AxeFire()
    {
        int axeCount = 3;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            for (int i = 0; i < axeCount; i++)
            {
                SkillAttack axe = _pool.UseSkill("Axe");

                axe.SetOrbit((360.0f / axeCount) * i);
                axe.Init(1, 1, 2, _pool, 2);
                axe.gameObject.SetActive(true);
            }

            yield return _spearRate;
        }
    }

    IEnumerator Co_DualBaldeFire()
    {
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack dBlade = _pool.UseSkill("DualBlade");

            dBlade.transform.position = transform.position;
            dBlade.Init(1, 1, 4, _pool);
            dBlade.gameObject.SetActive(true);

            yield return _spearRate;
        }
    }

    IEnumerator Co_MaceFire()
    {
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack mace = _pool.UseSkill("Mace");

            mace.transform.position = transform.position;
            mace.Init(1, 1, 4, _pool);
            mace.gameObject.SetActive(true);

            yield return _spearRate;
        }
    }

    IEnumerator Co_SpearFire()
    {
        while(_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack spear = _pool.UseSkill("Spear");            

            spear.transform.position = transform.position;
            spear.transform.rotation = Quaternion.Euler(0, 0, 90f);
            spear.Init(1, 1, 4, _pool);
            spear.gameObject.SetActive(true);


            yield return _spearRate;
        }
    }

    IEnumerator Co_TridentFire()
    {
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack trident = _pool.UseSkill("Trident");

            trident.transform.position = transform.position;
            trident.transform.up = _spawnDir.right;
            trident.Init(1, 1, 4, _pool);
            trident.gameObject.SetActive(true);

            yield return _spearRate;
        }
    }

    IEnumerator Co_WandFire()
    {
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack wand = _pool.UseSkill("Wand");

            wand.transform.position = transform.position;
            wand.transform.up = _spawnDir.right;
            wand.Init(1, 1, 4, _pool);
            wand.gameObject.SetActive(true);

            yield return _spearRate;
        }
    }
    #endregion

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

        //CoroutineActivate(tag);
    }

    void CoroutineActivate(string tag)
    {
        switch (tag)
        {
            case "Arrow":

                break;
            case "Axe":

                break;
            case "DualBlade":

                break;
            case "Mace":

                break;
            case "Spear":

                break;
            case "Trident":

                break;
            case "Wand":

                break;
        }
    }
}
