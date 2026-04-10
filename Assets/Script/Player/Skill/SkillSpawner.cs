using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SkillSpawner : MonoBehaviour
{
    // SO에서 파라미터를 받고 갯수에 맞춰서 풀에서 InitCreatePool호출 생산품의 Init에 파라미터 삽입
    [SerializeField] private SkillPool _pool;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _spawnDir;
    [SerializeField] private Camera _cam;
    [SerializeField] private SkillData_SO[] _skillData;
    [SerializeField] private SkillLevelRegistry _levelRegistry;

    [Serializable]
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
    [SerializeField] private List<SkillParameter> _debugList;
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
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            SkillAttack arrow = _pool.UseSkill(use.id);

            arrow.transform.position = transform.position;
            arrow.transform.up = _spawnDir.right;
            arrow.Init(use.id, use.damage, 1, use.speed, _pool, use.time);
            arrow.gameObject.SetActive(true);            
            yield return use.rate;
        }
    }

    IEnumerator Co_AxeFire(string id)
    {
        yield return null;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            for (int i = 0; i < use.count; i++)
            {
                SkillAttack axe = _pool.UseSkill(use.id);

                axe.SetOrbit((360.0f / use.count) * i);
                axe.Init(use.id, use.damage, 1, use.speed, _pool, use.time);
                axe.gameObject.SetActive(true);
            }

            yield return use.rate;
        }
    }

    IEnumerator Co_DualBaldeFire(string id)
    {
        yield return null;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            SkillAttack dBlade = _pool.UseSkill(use.id);

            dBlade.Init(use.id, use.damage, 1, use.speed, _pool, use.time);
            dBlade.SetComponent(transform);
            dBlade.gameObject.SetActive(true);

            yield return use.rate;
        }
    }

    IEnumerator Co_MaceFire(string id)
    {
        yield return null;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            SkillAttack mace = _pool.UseSkill(use.id);

            mace.transform.position = transform.position;
            mace.Init(use.id, use.damage, 1, use.speed, _pool, use.time);
            mace.gameObject.SetActive(true);

            yield return use.rate;
        }
    }

    IEnumerator Co_SpearFire(string id)
    {
        yield return null;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            for (int i = 0; i < use.count; i++)
            {
                SkillAttack spear = _pool.UseSkill(use.id);
                spear.transform.position = transform.position;
                spear.transform.rotation = Quaternion.Euler(0, 0, 90f + (360f / use.count) * i);
                spear.Init(use.id, use.damage, 1, use.speed, _pool, use.time);
                spear.gameObject.SetActive(true);
            }

            yield return use.rate;
        }
    }

    IEnumerator Co_TridentFire(string id)
    {
        yield return null;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            SkillAttack trident = _pool.UseSkill(use.id);

            trident.transform.position = transform.position;
            trident.transform.up = _spawnDir.right;
            trident.SetComponent(_player.transform, _cam);
            trident.Init(use.id, use.damage, 1, use.speed, _pool, use.time);
            trident.gameObject.SetActive(true);

            yield return use.rate;
        }
    }

    IEnumerator Co_WandFire(string id)
    {
        yield return null;
        float x;
        float y;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            for (int i = 0; i < use.count; i++)
            {
                SkillAttack wand = _pool.UseSkill(use.id);
                x = UnityEngine.Random.Range(-5f, 5f);
                y = UnityEngine.Random.Range(-3f, 3f);

                wand.transform.position = transform.position + new Vector3(x, y, 0);
                wand.Init(use.id, use.damage, 1, use.speed, _pool, use.time);
                wand.gameObject.SetActive(true);
            }

            yield return use.rate;
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

    public void GetEvolution(string id, string evolutionId)
    {
        _paramDict[evolutionId] = new SkillParameter();
        _paramDict[id] = _paramDict[evolutionId];
        foreach (var data in _skillData)
        {
            if (data.SkillID == evolutionId)
            {
                SkillAttack prefab = data.Prefab;
                _pool.InitSkillEvol(id, evolutionId, 16, prefab);
                break;
            }
        }
        SkillUpgrade(evolutionId, 1);
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
        _debugList = _paramDict.Values.ToList();
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
