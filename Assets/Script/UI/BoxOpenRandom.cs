using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpenRandom : MonoBehaviour
{
    [SerializeField]
    private ItemRegistry item;
    [SerializeField]
    LayerMask _playerBulletLayer;

    private GameObject mother;


    Collider2D[] _dmgCheckBuffer = new Collider2D[30];
    float _nextDmg = 0;
    float _radius = 1;
    Vector2 _offset;

    private void FixedUpdate()
    {
        CheckDamaged();
    }


    public void Setting(GameObject _mother)
    {
        mother = _mother;
    }

    void CheckDamaged()
    {
        if (Timer.Instance.TickTime == _nextDmg)
        {
            return;
        }

        _nextDmg = Timer.Instance.TickTime;

        int count = Physics2D.OverlapCircleNonAlloc((Vector2)transform.position + _offset,
            _radius,
            _dmgCheckBuffer,
            _playerBulletLayer);

        for (int i = 0; i < count; i++)
        {
            if (_dmgCheckBuffer[i].TryGetComponent(out IAttackables attackables))
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("무엇이 일어나고 있는것이지");
        RandomDrop();

        Destroy(this.gameObject);
    }

    void RandomDrop()
    {
        int gotcha = Random.Range(1, 5);

        GameObject obj = Instantiate(item.GetItemByID(gotcha).Prefab, mother.transform);

        obj.transform.position = transform.position;
    }
}
