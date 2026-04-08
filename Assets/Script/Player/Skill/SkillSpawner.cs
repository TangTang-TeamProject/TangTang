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
    [SerializeField] private SkillData_SO[] _skillData;
    [SerializeField] private SkillLevelRegistry _levelRegistry;

    public class SkillParameter
    {
        public string id;
        public float damage;
        public float speed;
        public float time;
        public float rate;
    }
    private Dictionary<string, SkillParameter> _parameterDict;

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
    private WaitForSeconds _tridentRate = new WaitForSeconds(2.2f);
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

    #region 스킬 코루틴
    IEnumerator Co_ArrowFire(string id)
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack arrow = _pool.UseSkill(id);

            arrow.transform.position = transform.position;
            arrow.transform.up = _spawnDir.right;
            arrow.Init(id, 1, 1, 4, _pool);
            arrow.gameObject.SetActive(true);

            yield return _arrowRate;
        }
    }

    IEnumerator Co_AxeFire(string id)
    {
        yield return null;
        int axeCount = 3;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            for (int i = 0; i < axeCount; i++)
            {
                SkillAttack axe = _pool.UseSkill(id);

                axe.SetOrbit((360.0f / axeCount) * i);
                axe.Init(id, 40, 1, 2, _pool, 2);
                axe.gameObject.SetActive(true);
            }

            yield return _axeRate;
        }
    }

    IEnumerator Co_DualBaldeFire(string id)
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack dBlade = _pool.UseSkill(id);

            dBlade.Init(id, 40, 1, 800, _pool, 0.3f);
            dBlade.gameObject.SetActive(true);

            yield return _dualBladeRate;
        }
    }

    IEnumerator Co_MaceFire(string id)
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack mace = _pool.UseSkill(id);

            mace.transform.position = transform.position;
            mace.Init(id, 40, 1, 4, _pool);
            mace.gameObject.SetActive(true);

            yield return _maceRate;
        }
    }

    IEnumerator Co_SpearFire(string id)
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack spear = _pool.UseSkill(id);
            /*
            for (int i = 0;, i < spearNum; i++)
            {
                spear.transform.rotation = Quaternion.Euler(0, 0, 90f + 360 / spearNum);
            }*/
            spear.transform.position = transform.position;
            spear.transform.rotation = Quaternion.Euler(0, 0, 90f);
            spear.Init(id, 40, 1, 4, _pool);
            spear.gameObject.SetActive(true);


            yield return _spearRate;
        }
    }

    IEnumerator Co_TridentFire(string id)
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack trident = _pool.UseSkill(id);

            trident.transform.position = transform.position;
            trident.transform.up = _spawnDir.right;
            trident.SetTrident(_cam, _player.transform);
            trident.Init(id, 40, 1, 7, _pool);
            trident.gameObject.SetActive(true);

            yield return _tridentRate;
        }
    }

    IEnumerator Co_WandFire(string id)
    {
        yield return null;
        float x;
        float y;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack wand = _pool.UseSkill(id);
            x = Random.Range(-5f, 5f);
            y = Random.Range(-3f, 3f);

            wand.transform.position = transform.position + new Vector3(x, y, 0);
            wand.Init(id, 40, 1, 4, _pool, 6.5f);
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

    public void GetSkill(string id, int level)
    {
        // 슬롯에서 id와 레벨을 받고 GetSkill한다.
        
        if (level == 1)
        {
            _parameterDict[id] = new SkillParameter();
            SkillUpgrade(id, level);
            SkillLearn(id, 8);
            return;
        }
        else
        {
            SkillUpgrade(id, level);
        }            
    }

    void SkillLearn(string id, int num)
    {
        bool isFind = false;
        foreach (var data in _skillData)
        {
            if (data.SkillID == id)
            {
                SkillAttack prefab = data.Prefab;
                _pool.InitCreateSkill(id, num, prefab);
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
            CPrint.Error($"{id}스킬 없음");
        }
        CoroutineActivate(id);
    }

    void SkillUpgrade(string id, int level)
    {
        SkillLevel_SO data = _levelRegistry.GetSkillDataByIDLevel(id, level);
        _parameterDict[id].id = data.SkillID;
        _parameterDict[id].damage = data.Damage;
        _parameterDict[id].speed = data.Speed;
        _parameterDict[id].time = data.AppearTime;
        _parameterDict[id].rate = data.DisAppearTime;
    }

    void CoroutineActivate(string id)
    {
        switch (id)
        {
            case "Arrow":
                _arrowCo = StartCoroutine(Co_ArrowFire(id));
                break;
            case "Axe":
                _axeCo = StartCoroutine(Co_AxeFire("Axe"));
                break;
            case "DualBlade":
                _dualBladeCo = StartCoroutine(Co_DualBaldeFire("DualBlade"));
                break;
            case "Mace":
                _maceCo = StartCoroutine(Co_MaceFire("Mace"));
                break;
            case "Spear":
                _spearCo = StartCoroutine(Co_SpearFire("Spear"));
                break;
            case "Trident":
                _tridentCo = StartCoroutine(Co_TridentFire("Trident"));
                break;
            case "Wand":
                _wandCo = StartCoroutine(Co_WandFire("Wand"));
                break;
        }
    }
}
