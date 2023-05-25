using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;
    public IItem item;
    public Image itemIcon;

    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemInfoText;

    private void Awake()
    {
        itemNameText = GameObject.Find("Canvas/InventoryUI/InfoPanel/ItemNameText").GetComponent<TextMeshProUGUI>();
        itemInfoText = GameObject.Find("Canvas/InventoryUI/InfoPanel/ItemExpText").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (item == null) return;

        bool isUse = item.Use();
        if (isUse)
        {
            Inventory.instance.RemoveItem(slotnum);
        }
    }

    public void OnMouse()
    {
        if (item == null) return;
        itemNameText.text = item.itemName;
        itemInfoText.text = item.itemInfo;
    }
}
