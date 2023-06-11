using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPool : MonoBehaviour
{
    [SerializeField]
    private PoolListSO _poolListSO;

    private void Awake()
    {
        PoolManager.instance = new PoolManager(transform);

        foreach (PoolingPair pair in _poolListSO.Pairs)
        {
            PoolManager.instance.CreatePool(pair.Prefab, pair.Count);
        }
    }
}
