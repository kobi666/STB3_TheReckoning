using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectorTest : MonoBehaviour
{
    Dictionary<Vector2, GameObject> towersWithPositions;


    private void Start() {
        towersWithPositions = TowerUtils.TowersWithPositionsFromParent(GameObject.FindGameObjectWithTag("TowerParent"));
    }
    
    
}
