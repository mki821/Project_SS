using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }
    public List<IItem> itemDB = new List<IItem>();

    public GameObject fieldItemPrefab;

    public void MakeItem(Vector3 vec)
    {
        GameObject go = Instantiate(fieldItemPrefab, vec, Quaternion.identity);
        go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, itemDB.Count)]);
    }

    public void MakeItem(Vector3 vec, int strVal, int endVal)
    {
        GameObject go = Instantiate(fieldItemPrefab, vec, Quaternion.identity);
        go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(strVal, endVal + 1)]);
    }
}
