using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/LevelUp")]
public class ItemLevelUp : ItemEffect
{
    public int gunType = 1;

    public override bool ExecuteRole()
    {
        switch (gunType)
        {
            case 1:
                GameObject.Find("Player/PlayerSprite/Gun").GetComponent<Gun>().LevelAR++;
                break;
            case 2:
                GameObject.Find("Player/PlayerSprite/Gun").GetComponent<Gun>().LevelShotgun++;
                break;
        }
        
        return true;
    }
}
