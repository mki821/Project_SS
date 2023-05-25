using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 3;

    [SerializeField]
    private float speed = 13;

    private void Awake()
    {
        StartCoroutine(Die());
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
