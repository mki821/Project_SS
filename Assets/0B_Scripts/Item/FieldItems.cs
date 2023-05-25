using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public IItem item;
    public SpriteRenderer image;

    public void SetItem(IItem _item)
    {
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;
        item.efts = _item.efts;

        image.sprite = item.itemImage;
    }

    public IItem GetItem()
    {
        return item;
    }

    public void DestoryItem()
    {
        Destroy(gameObject);
    }
}
