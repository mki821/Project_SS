using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomZombie : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private LayerMask player;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject boomPrefab;

    private int _curHp;

    private GameObject _player;
    private Image _hpGuage;

    //ªË¡¶?
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _hpGuage = transform.Find("Canvas/Background/Fill").GetComponent<Image>();
        _curHp = maxHp;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            Vector2 dir = _player.transform.position - transform.position;

            transform.position += (Vector3)dir.normalized * speed * Time.deltaTime;

            if (Check(player).CompareTag("Player"))
            {
                Die();
            }

            yield return new WaitForSeconds(Random.Range(Time.deltaTime * 0.5f, Time.deltaTime * 3));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet"))
        {
            int damage = collision.transform.GetComponent<Bullet>().bulletDamage;
            Destroy(collision.gameObject);
            _curHp -= damage;
            StartCoroutine(HitColor());

            _hpGuage.fillAmount -= damage / (float)maxHp;

            if (_curHp < 1)
            {
                Die();
            }
        }
    }

    private IEnumerator HitColor()
    {
        Color color = _spriteRenderer.color;

        _spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = color;
    }

    private void Die()
    {
        ItemDatabase.instance.MakeItem(transform.position, 0, 1);
        Instantiate(boomPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected Collider2D Check(LayerMask layer)
    {
        return Physics2D.OverlapCircle(transform.position, 1.5f, layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
