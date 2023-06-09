using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Zombie : PoolMono, IEnemy
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float dropPer = 0.1f;
    [SerializeField]
    private LayerMask player;
    [SerializeField]
    private GameObject dieEffectPrefab;

    private int _curHp;
    private bool _isFreeze = true;
    private bool _isAttack = false;
    private bool _dropItem = false;

    [SerializeField]
    private GameObject _player;
    private Image _hpGuage;
    private BoxCollider2D _boxCollider;

    //��������
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    Color color;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _hpGuage = transform.Find("Canvas/Background/Fill").GetComponent<Image>();
        _curHp = maxHp;

        color = _spriteRenderer.color;
    }

    private IEnumerator Move()
    {
        while (!GameManager.instance.gameEnd)
        {
            if (!_isFreeze && !_isAttack)
            {
                Vector2 dir = _player.transform.position - transform.position;

                transform.position += (Vector3)dir.normalized * speed * Time.deltaTime;

                if (!_isAttack)
                {
                    foreach (var item in Check(player))
                    {
                        if (item?.name == "Player")
                        {
                            AttackAnim();
                            StartCoroutine(Cooldown(2f));
                        }
                    }
                }
            }

            yield return new WaitForSeconds(Random.Range(Time.deltaTime * 0.5f, Time.deltaTime * 3));
        }
    }

    private void AttackAnim()
    {
        Sequence seq = DOTween.Sequence();

        _isAttack = true;
        Vector3 dir = (_player.transform.position - transform.position).normalized;

        seq.Append(transform.DOMove(transform.position + (-dir * 0.5f), 0.3f).SetEase(Ease.Linear));
        seq.Append(transform.DOMove(transform.position + (dir * 0.8f), 0.5f).SetEase(Ease.InCubic));
        seq.AppendCallback(() => Attack());
        seq.AppendCallback(() => _isAttack = false);
    }

    private void Attack()
    {
        foreach (var item in Check(player))
        {
            if (item?.name == "Player")
            {
                item?.GetComponent<PlayerStatus>().Damaged(damage);
            }
        }
    }

    private IEnumerator Cooldown(float time)
    {
        _isFreeze = true;

        yield return new WaitForSeconds(time);

        _isFreeze = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet"))
        {
            int damage = collision.transform.GetComponent<Bullet>().bulletDamage;
            Destroy(collision.gameObject);

            Damaged(damage);
            transform.DOMove(transform.position + (-(_player.transform.position - transform.position).normalized * 0.2f), 0.07f).SetEase(Ease.Linear);
        }
        else if (collision.transform.CompareTag("Swing"))
        {
            Damaged(collision.transform.parent.GetComponent<SwingAttack>().damage);
            transform.DOMove(transform.position + (-(_player.transform.position - transform.position).normalized * 0.7f), 0.3f).SetEase(Ease.OutExpo);
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
        if (!_dropItem)
        {
            float rnd = Random.Range(0, 1f);

            if(rnd < 0.05f)
            {
                _dropItem = true;
                ItemDatabase.instance.MakeItem(transform.position, 4, 5);
            }
            else if (rnd < dropPer)
            {
                _dropItem = true;
                ItemDatabase.instance.MakeItem(transform.position, 0, 3);
            }
        }
        Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySound("Zombie");
        UIManager.instance.Score += 100;
        EnemyAutoSpawn.instance.Count--;
        Destroy(gameObject);
    }

    public void Damaged(int damage)
    {
        _curHp -= damage;
        StartCoroutine(HitColor());

        _hpGuage.fillAmount -= damage / (float)maxHp;

        if (_curHp < 1)
        {
            Die();
        }
    }

    protected Collider2D[] Check(LayerMask layer)
    {
        return Physics2D.OverlapBoxAll(transform.position, new Vector2(1.2f, 1.2f), layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(1.2f, 1.2f));
    }

    public override void Init()
    {
        StopAllCoroutines();

        Debug.Log(GameManager.instance.player);
        _player = GameManager.instance.player;

        _boxCollider.enabled = true;

        StartCoroutine(Cooldown(0.5f));
        StartCoroutine(Move());
    }
}
