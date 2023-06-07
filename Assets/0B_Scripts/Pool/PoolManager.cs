using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private Dictionary<string, Pool<PoolMono>> _pools = new Dictionary<string, Pool<PoolMono>>();

    private Transform _parent;

    public PoolManager(Transform parent)
    {
        _parent = parent;
    }

    public void CreatePool(PoolMono prefab, int count = 10)
    {
        Pool<PoolMono> pool = new Pool<PoolMono>(prefab, _parent, count);
        _pools.Add(prefab.gameObject.name, pool);
    }

    public PoolMono Pop(string name, Vector3 pos, float angle)
    {
        if (!_pools.ContainsKey(name))
        {
            Debug.LogError($"Prefab does not exist on pool : {name}");
            return null;
        }

        PoolMono item = _pools[name].Pop(pos, angle);
        item.Init();
        return item;
    }

    public void Push(PoolMono obj)
    {
        _pools[obj.name].Push(obj);
    }
}
