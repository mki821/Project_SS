using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemKind { Manazine = 0, }

public class Item : MonoBehaviour
{
    public ItemKind itemKind;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (itemKind)
            {
                case ItemKind.Manazine:
                    Gun gun = collision.transform.Find("Gun").GetComponent<Gun>();
                    gun.Magazine++;
                    gun.TextSet();
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
