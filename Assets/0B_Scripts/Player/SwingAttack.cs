using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAttack : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        StartCoroutine(Swing());
    }

    private IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }
}
