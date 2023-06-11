using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomZombie : PoolMono, IEnemy
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
    private LayerMask enemy;
    [SerializeField]
    private GameObject boomPrefab;

    private int _curHp;
    private bool _dropItem = false;

    [SerializeField]
    private GameObject _player;
    private Image _hpGuage;
    private BoxCollider2D _boxCollider;

    //ªË¡¶?
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _hpGuage = transform.Find("Canvas/Background/Fill").GetComponent<Image>();
        _curHp = maxHp;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);

        while (!GameManager.instance.gameEnd)
        {
            Vector2 dir = _player.transform.position - transform.position;

            transform.position += (Vector3)dir.normalized * speed * Time.deltaTime;

            if (PlayerCheck())
            {
                Die();
            }

            yield return new WaitForSeconds(Random.Range(Time.deltaTime * 0.5f, Time.deltaTime * 3));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet"))
        {
            int damage = collision.transform.GetComponent<Bullet>().bulletDamage;
            Destroy(collision.gameObject);

            Damaged(damage);
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
        _boxCollider.enabled = false;
        float rnd = Random.Range(0, 1f);

        if (rnd < 0.05f)
        {
            _dropItem = true;
            ItemDatabase.instance.MakeItem(transform.position, 4, 5);
        }
        else if (!_dropItem && rnd < dropPer)
        {
            _dropItem = true;
            ItemDatabase.instance.MakeItem(transform.position, 0, 1);
        }

        CameraManager.instance.CameraShake(10, 2);
        Instantiate(boomPrefab, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySound("Explosion");
        UIManager.instance.Score += 150;
        PlayerCheck()?.GetComponent<PlayerStatus>().Damaged(damage);
        foreach (var item in Check(enemy))
            item?.GetComponent<IEnemy>().Damaged(damage * 2);
        EnemyAutoSpawn.instance.Count--;
        Destroy(gameObject);
    }

    protected Collider2D PlayerCheck() => Physics2D.OverlapCircle(transform.position, 2f, player);

    protected Collider2D[] Check(LayerMask layer)
    {
        return Physics2D.OverlapCircleAll(transform.position, 2f, layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.8f);
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

    public override void Init()
    {
        StopAllCoroutines();

        _player = GameManager.instance.player;

        _boxCollider.enabled = true;

        StartCoroutine(Move());
    }
}
