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
        public int count;
        public float time;
        public float cool;
        public WaitForSeconds rate;
    }
    private Dictionary<string, SkillParameter> _paramDict = new Dictionary<string, SkillParameter>();
    private Dictionary<string, Coroutine> _coDict = new Dictionary<string, Coroutine>();

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
            arrow.Init(id, _paramDict[id].damage, 1, _paramDict[id].speed, _pool, _paramDict[id].time);
            arrow.gameObject.SetActive(true);            
            yield return _paramDict[id].rate;
        }
    }

    IEnumerator Co_AxeFire(string id)
    {
        CPrint.Log("도끼 코루틴 시작");
        yield return null;
        int count;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            count = _paramDict[id].count;
            for (int i = 0; i < count; i++)
            {
                SkillAttack axe = _pool.UseSkill(id);

                axe.SetOrbit((360.0f / count) * i);
                axe.Init(id, _paramDict[id].damage, 1, _paramDict[id].speed, _pool, _paramDict[id].time);
                axe.gameObject.SetActive(true);
            }

            yield return _paramDict[id].rate;
        }
    }

    IEnumerator Co_DualBaldeFire(string id)
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack dBlade = _pool.UseSkill(id);

            dBlade.Init(id, _paramDict[id].damage, 1, _paramDict[id].speed, _pool, _paramDict[id].time);
            dBlade.SetComponent(transform);
            dBlade.gameObject.SetActive(true);

            yield return _paramDict[id].rate;
        }
    }

    IEnumerator Co_MaceFire(string id)
    {
        yield return null;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            SkillAttack mace = _pool.UseSkill(id);

            mace.transform.position = transform.position;
            mace.Init(id, _paramDict[id].damage, 1, _paramDict[id].speed, _pool, _paramDict[id].time);
            mace.gameObject.SetActive(true);

            yield return _paramDict[id].rate;
        }
    }

    IEnumerator Co_SpearFire(string id)
    {
        yield return null;
        int count;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            count = _paramDict[id].count;
            for (int i = 0; i < count; i++)
            {
                SkillAttack spear = _pool.UseSkill(id);
                spear.transform.position = transform.position;
                spear.transform.rotation = Quaternion.Euler(0, 0, 90f + (360f / count) * i);
                spear.Init(id, _paramDict[id].damage, 1, _paramDict[id].speed, _pool, _paramDict[id].time);
                spear.gameObject.SetActive(true);
            }

            yield return _paramDict[id].rate;
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
            trident.SetComponent(_player.transform, _cam);
            trident.Init(id, _paramDict[id].damage, 1, _paramDict[id].speed, _pool, _paramDict[id].time);
            trident.gameObject.SetActive(true);

            yield return _paramDict[id].rate;
        }
    }

    IEnumerator Co_WandFire(string id)
    {
        yield return null;
        float x;
        float y;
        int count;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            count = _paramDict[id].count;

            for (int i = 0; i < count; i++)
            {
                SkillAttack wand = _pool.UseSkill(id);
                x = Random.Range(-5f, 5f);
                y = Random.Range(-3f, 3f);

                wand.transform.position = transform.position + new Vector3(x, y, 0);
                wand.Init(id, _paramDict[id].damage, 1, _paramDict[id].speed, _pool, _paramDict[id].time);
                wand.gameObject.SetActive(true);
            }

            yield return _paramDict[id].rate;
        }
    }
    #endregion

    private void OnDisable()
    {
        foreach (Coroutine co in _coDict.Values)
        {
            StopCoroutine(co);
        }
        _coDict.Clear();
    }

    public void GetSkill(string id, int level)
    {
        // 슬롯에서 id와 레벨을 받고 GetSkill한다.
        
        if (level == 1)
        {
            _paramDict[id] = new SkillParameter();
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
        _paramDict[id].id = data.SkillID;
        _paramDict[id].damage = data.Damage;
        _paramDict[id].speed = data.Speed;
        _paramDict[id].count = data.Count;
        _paramDict[id].time = data.AppearTime;
        _paramDict[id].cool = data.DisAppearTime;
        _paramDict[id].rate = new WaitForSeconds(_paramDict[id].time + _paramDict[id].cool);
    }

    void CoroutineActivate(string id)
    {
        switch (id)
        {
            case "Arrow":
                _coDict[id] = StartCoroutine(Co_ArrowFire(id));
                break;
            case "Axe":
                _coDict[id] = StartCoroutine(Co_AxeFire(id));
                break;
            case "DualBlade":
                _coDict[id] = StartCoroutine(Co_DualBaldeFire(id));
                break;
            case "Mace":
                _coDict[id] = StartCoroutine(Co_MaceFire(id));
                break;
            case "Spear":
                _coDict[id] = StartCoroutine(Co_SpearFire(id));
                break;
            case "Trident":
                _coDict[id] = StartCoroutine(Co_TridentFire(id));
                break;
            case "Wand":
                _coDict[id] = StartCoroutine(Co_WandFire(id));
                break;
        }
    }
}
