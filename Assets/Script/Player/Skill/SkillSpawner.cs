using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    // SO에서 파라미터를 받고 갯수에 맞춰서 풀에서 InitCreatePool호출 생산품의 Init에 파라미터 삽입
    [SerializeField] private SkillPool _pool;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _spawnDir;
    [SerializeField] private Camera _cam;


    private WaitForSeconds _arrowRate = new WaitForSeconds(2.2f);
    private Coroutine _arrowCo;
    private WaitForSeconds _axeRate = new WaitForSeconds(2.2f);
    private Coroutine _axeCo;
    private WaitForSeconds _dualBladeRate = new WaitForSeconds(2.2f);
    private Coroutine _dualBladeCo;
    private WaitForSeconds _maceRate = new WaitForSeconds(2.2f);
    private Coroutine _maceCo;
    private WaitForSeconds _spearRate= new WaitForSeconds(2.2f);
    private Coroutine _spearCo;
    private WaitForSeconds _tridentRate = new WaitForSeconds(52.2f);
    private Coroutine _tridentCo;
    private WaitForSeconds _wandRate = new WaitForSeconds(2.2f);
    private Coroutine _wandCo;

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

        if (_cam == null)
        {
            _cam = Camera.main;
        }

    }

    void Start()
    {
        GetSkill("Axe", 6);
        GetSkill("DualBlade", 6);
        GetSkill("Mace", 5);
        GetSkill("Spear", 5);
        GetSkill("Trident", 5);
        GetSkill("Wand", 6);

        _axeCo = StartCoroutine(Co_AxeFire());
        _dualBladeCo = StartCoroutine(Co_DualBaldeFire());
        _maceCo = StartCoroutine(Co_MaceFire());
        _spearCo = StartCoroutine(Co_SpearFire());
        _tridentCo = StartCoroutine(Co_TridentFire());
        _wandCo = StartCoroutine(Co_WandFire());
    }

    // 임시로 만든 로직이므로 SO같은 데이터가 들어오면 수정될 것
    #region 스킬 코루틴
    IEnumerator Co_ArrowFire()
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack arrow = _pool.UseSkill("Arrow");

            arrow.transform.position = transform.position;
            arrow.transform.up = _spawnDir.right;
            arrow.Init(1, 1, 4, _pool);
            arrow.gameObject.SetActive(true);

            yield return _arrowRate;
        }
    }

    IEnumerator Co_AxeFire()
    {
        yield return null;
        int axeCount = 3;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            for (int i = 0; i < axeCount; i++)
            {
                SkillAttack axe = _pool.UseSkill("Axe");

                axe.SetOrbit((360.0f / axeCount) * i);
                axe.Init(40, 1, 2, _pool, 2);
                axe.gameObject.SetActive(true);
            }

            yield return _axeRate;
        }
    }

    IEnumerator Co_DualBaldeFire()
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack dBlade = _pool.UseSkill("DualBlade");

            dBlade.Init(40, 1, 800, _pool, 0.3f);
            dBlade.gameObject.SetActive(true);

            yield return _dualBladeRate;
        }
    }

    IEnumerator Co_MaceFire()
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack mace = _pool.UseSkill("Mace");

            mace.transform.position = transform.position;
            mace.Init(40, 1, 4, _pool);
            mace.gameObject.SetActive(true);

            yield return _maceRate;
        }
    }

    IEnumerator Co_SpearFire()
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack spear = _pool.UseSkill("Spear");
            /*
            for (int i = 0;, i < spearNum; i++)
            {
                spear.transform.rotation = Quaternion.Euler(0, 0, 90f + 360 / spearNum);
            }*/
            spear.transform.position = transform.position;
            spear.transform.rotation = Quaternion.Euler(0, 0, 90f);
            spear.Init(40, 1, 4, _pool);
            spear.gameObject.SetActive(true);


            yield return _spearRate;
        }
    }

    IEnumerator Co_TridentFire()
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack trident = _pool.UseSkill("Trident");

            trident.transform.position = transform.position;
            trident.transform.up = _spawnDir.right;
            trident.SetTrident(_cam, _player.transform);
            trident.Init(40, 1, 7, _pool, 50);
            trident.gameObject.SetActive(true);

            yield return _tridentRate;
        }
    }

    IEnumerator Co_WandFire()
    {
        yield return null;
        float x;
        float y;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack wand = _pool.UseSkill("Wand");
            x = Random.Range(-5f, 5f);
            y = Random.Range(-3f, 3f);

            wand.transform.position = transform.position + new Vector3(x, y, 0);
            wand.Init(40, 1, 4, _pool, 6.5f);
            wand.gameObject.SetActive(true);

            yield return _wandRate;
        }
    }
    #endregion

    private void OnDisable()
    {
        if (_arrowCo != null)
        {
            StopCoroutine(_arrowCo);
            _arrowCo = null;
        }
        if (_axeCo != null)
        {
            StopCoroutine(_axeCo);
            _axeCo = null;
        }
        if (_dualBladeCo != null)
        {
            StopCoroutine(_dualBladeCo);
            _dualBladeCo = null;
        }
        if (_maceCo != null)
        {
            StopCoroutine(_maceCo);
            _maceCo = null;
        }
        if (_spearCo != null)
        {
            StopCoroutine(_spearCo);
            _spearCo = null;
        }
        if (_tridentCo != null)
        {
            StopCoroutine(_tridentCo);
            _tridentCo = null;
        }
        if (_wandCo != null)
        {
            StopCoroutine(_wandCo);
            _wandCo = null;
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
