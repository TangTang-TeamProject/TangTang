using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemFactory : MonoBehaviour
{
    [Header("3 가지 종류의 GemPool 연결")]
    [SerializeField] private List<GemPool> _pool; // gemPool 3가지 종류 모두 연결해야 함.
    
    public ExpGem CreateGem(Vector2 pos, GemType gemType)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (_pool[i].GetPoolType() == gemType) // 해당 type 의 gempool 에서 생성
            {
                ExpGem gem = _pool[i].GetGem(transform);
                gem.Init(_pool[i]);

                gem.transform.position = pos;
                return gem;
            }
        }

        CPrint.Error($"{this} : GemPool 연결 오류");
        return null;
    }
}
