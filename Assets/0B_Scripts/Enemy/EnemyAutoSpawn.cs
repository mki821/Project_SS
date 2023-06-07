using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public string name;
    public float probability;
}

public class EnemyAutoSpawn : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private float spawnTimeMin;
    [SerializeField]
    private float spawnTimeMax;

    private float sum = 0;

    private PlayerMove _playerMove;

    [SerializeField]
    private List<EnemyInfo> enemyInfo = new();

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        foreach (var item in enemyInfo)
        {
            sum += item.probability;
        }

        StartCoroutine(EnemySpawn());
    }

    private IEnumerator EnemySpawn()
    {
        while (true)
        {
            float e = Random.Range(0, 360f);
            if (_playerMove.dir.x != 0 && _playerMove.dir.y != 0)
            {
                float a = Mathf.Sqrt(1);
                float b = Mathf.Sqrt(Mathf.Pow(_playerMove.dir.x, 2) + Mathf.Pow(_playerMove.dir.y, 2));
                float c = _playerMove.dir.y;
                float d = Mathf.Acos(c / (a * b));
                e = (d * Mathf.PI) / 180;
            }
            else if (_playerMove.dir.x == 0 && _playerMove.dir.y == 1) e = 0;
            else if (_playerMove.dir.x == 1 && _playerMove.dir.y == 0) e = 90;
            else if (_playerMove.dir.x == 0 && _playerMove.dir.y == -1) e = 180;
            else if (_playerMove.dir.x == -1 && _playerMove.dir.y == 0) e = 270;

            float rad = Random.Range(e - 80, e + 80);
            if (rad < 0) rad += 360;
            else if (rad > 360) rad -= 360;
            rad *= Mathf.Deg2Rad;
            float x = (radius + Random.Range(0, 8)) * Mathf.Sin(rad);
            float y = (radius + Random.Range(0, 8)) * Mathf.Cos(rad);

            float rnd = Random.Range(0, sum);
            Debug.Log(enemyInfo[0].name);
            if(rnd <= enemyInfo[0].probability) PoolManager.instance.Pop(enemyInfo[0].name, transform.position + new Vector3(x, y), 0);
            else if(rnd <= enemyInfo[1].probability + enemyInfo[0].probability) PoolManager.instance.Pop(enemyInfo[1].name, transform.position + new Vector3(x, y), 0);

            yield return new WaitForSeconds(Random.Range(spawnTimeMin, spawnTimeMax));
        }
    }
}
