using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Magazine")]
public class ItemMagazineEft : ItemEffect
{
    public int magazineKind = 0;

    public override bool ExecuteRole()
    {
        GameObject.Find("Player/PlayerSprite/Gun").GetComponent<Gun>().MagazinePlus(magazineKind);
        return true;
    }
}
