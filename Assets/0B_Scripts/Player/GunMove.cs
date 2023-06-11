using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunMove : MonoBehaviour
{
    public Vector2 _v2;

    [SerializeField]
    private GameObject swingAttackPrefab;

    private Vector2 _mousePos;

    private Gun _gun;

    private void Awake()
    {
        _gun = GetComponent<Gun>();
    }

    private void Update()
    {
        MouseMove();
        MousePositionSet();
    }

    private void MouseMove()
    {
        if (Time.timeScale != 0 && !_gun.isAttack)
        {
            transform.rotation = Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg) + 90);

            if (transform.parent.position.x < _mousePos.x)
            {
                transform.parent.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.parent.position.x > _mousePos.x)
            {
                transform.parent.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private void MousePositionSet()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _v2 = _mousePos - (Vector2)transform.position;
    }

    public void Swing()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotateQuaternion(Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg) + 40), 0.15f));
        seq.AppendCallback(() => StartCoroutine(SwingAtk()));
        seq.Append(transform.DORotateQuaternion(Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg) + 140), 0.1f).SetEase(Ease.InQuad));
        seq.Append(transform.DORotateQuaternion(Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg) + 90), 0.25f).SetEase(Ease.InCubic));
    }

    private IEnumerator SwingAtk()
    {
        yield return new WaitForSeconds(0.05f);

        Instantiate(swingAttackPrefab, transform.position, Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg)), transform.parent);
    }
}
