using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f;

    private Rigidbody2D _rb;
    private GameObject _target;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_target == null)
            return;

        Vector2 dir = (_target.transform.position - transform.position).normalized;
        _rb.velocity = dir * _moveSpeed;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Die();            
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        EnemySpawner.Instance.Return(this);
    }
}
