using System;
using UnityEngine;

[Serializable]
public class TowerPlacementItem
{
    public TowerController TowerPrefab;
    public int Cost;
    //show later
    [HideInInspector]
    public Sprite ItemSprite;
}
