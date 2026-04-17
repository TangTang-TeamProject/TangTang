using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    // SO에서 파라미터를 받고 갯수에 맞춰서 풀에서 InitCreatePool호출 생산품의 Init에 파라미터 삽입
    [SerializeField] private SkillPool _pool;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _spawnDir;
    [SerializeField] private InfiniteMap _map;
    [SerializeField] private Camera _cam;
    [SerializeField] private SkillData_SO[] _skillData;
    [SerializeField] private SkillLevelRegistry _levelRegistry;

    [System.Serializable]
    public class SkillParameter
    {
        public string id;
        public float damage;
        public float speed;
        public float range = 1;
        public int count;
        public float time;
        public float cool;
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

        if (_map == null)
        {
            _map = FindAnyObjectByType<InfiniteMap>();
        }

        if (_cam == null)
        {
            _cam = Camera.main;
        }

    }

    #region 스킬 코루틴
    IEnumerator Co_AxeFire(string id)
    {
        yield return null;
        float time;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            time = use.time * _player.Duration;
            for (int i = 0; i < use.count; i++)
            {
                SkillAttack axe = _pool.UseSkill(use.id);

                axe.SetComponent(transform);
                axe.SetOrbit((360.0f / use.count) * i);
                axe.Init(use.id, use.damage, _player.Attack, use.speed, use.range, _pool, _player, time, _player.Range);
                axe.gameObject.SetActive(true);
            }
            
            if (use.id == "AxeEvo")
            {
                _coDict.Remove(id);
                yield break;
            }
            
            float rate = time + (use.cool * _player.Cool);
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator Co_DaggerFire(string id)
    {
        yield return null;
        float time;
        int x = 1;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            time = use.time * _player.Duration;

            SkillAttack dagger = _pool.UseSkill(use.id);

            dagger.transform.position = transform.position;
            dagger.transform.up = _spawnDir.right;

            if (use.id == "DaggerEvo")
            {
                dagger.transform.position += dagger.transform.right * 0.1f * x;
                x *= -1;
            }
            dagger.Init(use.id, use.damage, _player.Attack, use.speed, use.range, _pool, _player, time, _player.Range);
            dagger.gameObject.SetActive(true);
            SoundManager.Instance.PlaySfx(ESfxType.Dagger);

            float rate = time + (use.cool * _player.Cool);
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator Co_DualBaldeFire(string id)
    {
        yield return null;
        float time;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            time = use.time * _player.Duration;
            SkillAttack dBlade = _pool.UseSkill(use.id);

            dBlade.Init(use.id, use.damage, _player.Attack, use.speed, use.range, _pool, _player, time, _player.Range);
            dBlade.SetComponent(transform);
            dBlade.gameObject.SetActive(true);
            SoundManager.Instance.PlaySfx(ESfxType.DualBlade);

            float rate = time + (use.cool * _player.Cool);
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator Co_MaceFire(string id)
    {
        yield return null;
        float time;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            time = use.time * _player.Duration;
            SkillAttack mace = _pool.UseSkill(use.id);

            mace.transform.position = transform.position;
            mace.Init(use.id, use.damage, _player.Attack, _spawnDir.localPosition.x < 0 ?
                -use.speed : use.speed, use.range, _pool, _player, time, _player.Range);
            mace.gameObject.SetActive(true);
            SoundManager.Instance.PlaySfx(ESfxType.Mace);

            float rate = time + (use.cool * _player.Cool);
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator Co_SpearFire(string id)
    {
        yield return null;
        float time;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            time = use.time * _player.Duration;
            for (int i = 0; i < use.count; i++)
            {
                SkillAttack spear = _pool.UseSkill(use.id);
                spear.transform.position = transform.position;
                // 왼쪽을 바라보면 Z축이 90f에서 시작, 오른쪽으면 270 -> -90f에서 시작
                spear.transform.rotation = _spawnDir.localPosition.x < 0 ?
                    Quaternion.Euler(0, 0, 90f + (360f / use.count) * i) : Quaternion.Euler(0, 0, 270f + (360f / use.count) * i);
                spear.Init(use.id, use.damage, _player.Attack, use.speed, use.range, _pool, _player, time, _player.Range);
                spear.gameObject.SetActive(true);
            }
            SoundManager.Instance.PlaySfx(ESfxType.Spear);

            float rate = time + (use.cool * _player.Cool);
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator Co_TridentFire(string id)
    {
        yield return null;
        float time;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            time = use.time * _player.Duration;
            SkillAttack trident = _pool.UseSkill(use.id);

            trident.transform.position = transform.position;
            trident.transform.up = _spawnDir.right;
            trident.SetComponent(_player.transform, _cam);
            trident.Init(use.id, use.damage, _player.Attack, use.speed, use.range, _pool, _player, time, _player.Range);
            trident.gameObject.SetActive(true);
            SoundManager.Instance.PlaySfx(ESfxType.Trident);

            float rate = time + (use.cool * _player.Cool);
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator Co_WandFire(string id)
    {
        yield return null;
        float x;
        float y;
        float time;
        SkillParameter use;
        while (_player.PlayerState != Player.EPlayerState.Dead)
        {
            use = _paramDict[id];
            time = use.time * _player.Duration;
            GetWall(out float top, out float bottom, out float right, out float left);
            for (int i = 0; i < use.count; i++)
            {
                SkillAttack wand = _pool.UseSkill(use.id);
                x = Random.Range(left, right);
                y = Random.Range(bottom, top);

                wand.transform.position = transform.position + new Vector3(x, y, 0);
                wand.SetMap(_map);
                wand.Init(use.id, use.damage, _player.Attack, use.speed, use.range, _pool, _player, time, _player.Range);
                wand.gameObject.SetActive(true);
            }
            float rate = time + (use.cool * _player.Cool);
            yield return new WaitForSeconds(rate);
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
        _paramDict[id].range = data.Range;
        _paramDict[id].count = data.Count;
        _paramDict[id].time = data.AppearTime;
        _paramDict[id].cool = data.DisAppearTime;
        _debugList = _paramDict.Values.ToList();
    }

    void CoroutineActivate(string id)
    {
        switch (id)
        {
            case "Axe":
                _coDict[id] = StartCoroutine(Co_AxeFire(id));
                break;
            case "Dagger":
                _coDict[id] = StartCoroutine(Co_DaggerFire(id));
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
            case "DaggerEvo":
                _coDict[id] = StartCoroutine(Co_DaggerFire(id));
                break;
            case "AxeEvo":
                _coDict[id] = StartCoroutine(Co_AxeFire(id));
                break;
            case "DualBladeEvo":
                _coDict[id] = StartCoroutine(Co_DualBaldeFire(id));
                break;
            case "MaceEvo":
                _coDict[id] = StartCoroutine(Co_MaceFire(id));
                break;
            case "SpearEvo":
                _coDict[id] = StartCoroutine(Co_SpearFire(id));
                break;
            case "TridentEvo":
                _coDict[id] = StartCoroutine(Co_TridentFire(id));
                break;
            case "WandEvo":
                _coDict[id] = StartCoroutine(Co_WandFire(id));
                break;
        }
    }

    void GetWall(out float top, out float bottom, out float right, out float left)
    {
        float camHeight = _cam.orthographicSize;
        float camWidth = camHeight * _cam.aspect;

        top = camHeight-1;
        bottom = -camHeight+1f;
        right = camWidth-1;
        left = -camWidth+1;
    }
}
