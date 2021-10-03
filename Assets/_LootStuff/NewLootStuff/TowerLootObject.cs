using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class TowerLootObject : NewLootObject
{
    [Required]
    public TowerController TowerPrefab;
    
    public  override Sprite ObjectSprite
    {
        get => TowerPrefab?.TowerSprite;
    }
    
    public override void OnLootObjectSelect(LootSelector lootSelector)
    {
        if (GameManager.Instance.PlayerTowersManager.SlotAvailable())
        {
            foreach (var v in TowerUtils.ButtonDirectionsNamesArray)
            {
                if (GameManager.Instance.PlayerTowersManager.GetTowerByButtonName(v) == null)
                {
                    GameManager.Instance.PlayerTowersManager.SetTowerByButtonName(v,TowerPrefab);
                }
            }
        }
        else
        {
            lootSelector.SelectedTowerController = TowerPrefab;
            lootSelector.OpenTowerPlacementMenu(this);
        }
    }

    
}
