using System;
using System.Collections;
using System.Collections.Generic;
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
