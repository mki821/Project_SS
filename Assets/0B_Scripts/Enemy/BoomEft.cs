using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEft : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;

    private CircleCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collider.enabled = false;

            collision.GetComponent<PlayerStatus>().Damaged(damage);
        }
    }
}
