using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpenRandom : MonoBehaviour, IDamagables
{
    [SerializeField]
    private ItemRegistry item;

    private GameObject mother;

    public void Hit(float damage)
    {
        Die();
    }

    public void Die()
    {
        RandomDrop();

        Destroy(this.gameObject);
    }

    public void Setting(GameObject _mother)
    {
        mother = _mother;
    }

    void RandomDrop()
    {
        int gotcha = Random.Range(1, 5);

        GameObject obj = Instantiate(item.GetItemByID(gotcha).Prefab, mother.transform);

        obj.transform.position = transform.position;
    }
}
