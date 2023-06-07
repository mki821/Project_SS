using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PoolListSO _poolListSO;

    private void Awake()
    {
        CreatePoolManager();
    }

    private void CreatePoolManager()
    {
        PoolManager.instance = new PoolManager(transform);

        foreach (PoolingPair pair in _poolListSO.Pairs)
        {
            PoolManager.instance.CreatePool(pair.Prefab, pair.Count);
        }
    }
}
