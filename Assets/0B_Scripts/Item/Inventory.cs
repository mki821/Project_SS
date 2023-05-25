using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<IItem> items = new List<IItem>();

    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange.Invoke(slotCnt);
        }
    }

    private void Start()
    {
        SlotCnt = 4;
    }

    public bool AddItem(IItem _item)
    {
        if(items.Count < SlotCnt)
        {
            items.Add(_item);
            if(onChangeItem != null)
            onChangeItem.Invoke();
            return true;
        }
        return false;
    }

    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
        onChangeItem.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fielditems = collision.GetComponent<FieldItems>();
            if (AddItem(fielditems.GetItem()))
                fielditems.DestoryItem();
        }
    }
}
