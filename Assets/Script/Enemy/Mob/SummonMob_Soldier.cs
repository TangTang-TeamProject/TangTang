using UnityEngine;

public class SummonMob_Soldier : BaseEnemy
{
  
    protected override void Start()
    {
        ItemManager.instance.Bomb += Die;
    }

    protected override void Update()
    {
        base.Update();

        if (!CanUpdate())
        {
            return;
        }

        Chase();
        CheckDamaged();
    }

    public override void Init(EnemyPool pool, int idx)
    {
        if (pool != null)
        {
            _pool = pool;
        }
        _idx = idx;

        _id = _monsterData.EnemyID;
        _maxHp = _monsterData.HP;
        _contactDamage = _monsterData.ContactDamage;
        _speed = _monsterData.MoveSpeed;
        _atkCycle = _monsterData.ATKCycle;
        _bulletSpeed = _monsterData.BulletSpeed;
        _expDrop = _monsterData.ExpDrop;
        _mobType = _monsterData.EnemyType;
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.

        if (_sr == null)
        {
            _sr = GetComponent<SpriteRenderer>();
        }
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출
              
        Destroy(gameObject);
    }
}
