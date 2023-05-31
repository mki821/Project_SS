using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolingPair
{
    public PoolMono Prefab;
    public int Count;
}

[CreateAssetMenu(menuName = "SO/PoolList")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingPair> Pools;
}
