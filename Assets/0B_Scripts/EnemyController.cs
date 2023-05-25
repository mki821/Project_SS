using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int damage;
    [SerializeField]
    private LayerMask player;
    [SerializeField]
    private GameObject itemPrefab;

    private int _curHp;
    private bool _isFreeze = true;

    private GameObject _player;
    [SerializeField]
    private Image _hpGuage;

    //삭제예정
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    Color color;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _hpGuage = transform.Find("Canvas/Background/Fill").GetComponent<Image>();
        _curHp = maxHp;

        StartCoroutine(Cooldown(1f));
        StartCoroutine(Move());

        color = _spriteRenderer.color;
    }

    private void Update()
    {
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if (!_isFreeze)
            {
                Vector2 dir = _player.transform.position - transform.position;

                transform.position += (Vector3)dir.normalized * speed * Time.deltaTime;

                if (Check(player).CompareTag("Player"))
                {
                    Debug.Log(Check(player));
                    Check(player).GetComponent<PlayerStatus>().Damaged(damage);
                    StartCoroutine(Cooldown(1f));
                }
            }

            yield return new WaitForSeconds(Random.Range(Time.deltaTime * 0.5f, Time.deltaTime * 3));
        }
    }

    private IEnumerator Cooldown(float time)
    {
        _isFreeze = true;

        yield return new WaitForSeconds(time);

        _isFreeze = false;
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

            if(_curHp < 1)
            {
                Die();
            }
        }
    }

    private IEnumerator HitColor()
    {
        _spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = color;
    }

    private void Die()
    {
        ItemDatabase.instance.MakeItem(transform.position, 0, 1);
        Destroy(gameObject);
    }

    protected Collider2D Check(LayerMask layer)
    {
        return Physics2D.OverlapBox(transform.position, new Vector2(1.2f, 1.2f), layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(1.2f, 1.2f));
    }
}
