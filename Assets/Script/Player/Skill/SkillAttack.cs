using UnityEngine;

public abstract class SkillAttack : MonoBehaviour, IAttackables
{
    protected SkillPool _pool;
    protected Player _player;

    protected string _id;
    protected float _damage;
    protected float _baseDamage;
    protected float _keepTime;
    protected float _speed;
    protected float _range;
    protected float _remainTime;
    protected float _spinZ;
    protected bool _isSpin = false;
    protected float _stunTime;
    protected Vector3 _baseScale;

    public float Damage => _damage;
    public float Stun => _stunTime;

    private void Update()
    {
        Move();
        if (_isSpin)
        {
            Rotate();
        }
        
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            ReturnPool();
        }
        
    }

    public void Init(string id, float damage, float playerAttack, float speed, float range,
        SkillPool pool, Player player, float time = 5.0f, float playerRange = 1.0f)
    {
        _id = id;
        _pool = pool;
        _baseDamage = damage;
        _damage = playerAttack * _baseDamage;
        _speed = speed;
        _range = range;
        transform.localScale = _baseScale * _range * playerRange;
        _keepTime = time;
        _remainTime = _keepTime;
        _player = player;
        _player.OnAttackChange += DamageChange;
        _player.OnRangeChange += RangeChange;
    }

    protected virtual void Move() { }
    protected virtual void Rotate() { }
    public virtual void GetDestroy() { }
    public virtual void SetOrbit(float dist) { }
    public virtual void SetMap(InfiniteMap map) { }
    public virtual void SetComponent(Transform center, Camera cam = null) { }

    // 아티팩트에 의해 이미 발사된 객체의 파라미터값 변경시
    public void DamageChange(float playerAttack)
    {
        _damage = playerAttack * _baseDamage;
    }
    
    public void RangeChange(float playerRange)
    {
        transform.localScale = _baseScale * _range * playerRange;
    }

    protected void ReturnPool()
    {
        _player.OnAttackChange -= DamageChange;
        _player.OnRangeChange -= RangeChange;
        _remainTime = _keepTime;
        _pool.ReturnPool(_id, this);
    }
}
