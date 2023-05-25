using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private int maxHp = 10;
    [SerializeField]
    private int maxFood = 30;

    private int curHp;
    private int curFood;

    private Image _hpBar;
    private Image _foodBar;

    //Áö¿ï°Å
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public int CurHp { get => curHp; set => curHp = Mathf.Clamp(value, 0, maxHp); }
    public int CurFood { get => curFood; set => curFood = Mathf.Clamp(value, 0, maxFood); }

    private void Awake()
    {
        CurHp = maxHp;
        CurFood = maxFood;
        _hpBar = GameObject.Find("Canvas/Status/HpBackground/Hp").GetComponent<Image>();
        _foodBar = GameObject.Find("Canvas/Status/FoodBackground/Food").GetComponent<Image>();

        StartCoroutine(AutoHeal());
    }

    private IEnumerator AutoHeal()
    {
        while (true)
        {
            yield return new WaitUntil(() => CurHp != maxHp);
                while (CurHp != maxHp)
            {
                yield return new WaitForSeconds(Random.Range(4, 6));

                CurHp += 1;
                CurFood -= 2;
                UISet();
            }
        }
    }

    public void Damaged(int damage)
    {
        CurHp -= damage;
        Debug.Log("Player : " + CurHp);

        StartCoroutine(HitColor());
        UISet();
    }

    private IEnumerator HitColor()
    {
        Color def = _spriteRenderer.color;
        _spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.2f);

        _spriteRenderer.color = def;
    }

    private void UISet()
    {
        _hpBar.fillAmount = (float)CurHp / maxHp;
        _foodBar.fillAmount = (float)CurFood / maxFood;
    }
}
