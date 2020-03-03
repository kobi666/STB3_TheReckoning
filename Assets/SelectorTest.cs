using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectorTest : MonoBehaviour
{
    public Dictionary<Vector2, GameObject> towersWithPositions;
    public static SelectorTest instance;

    private void Awake() {
        instance = this;
        towersWithPositions = TowerUtils.TowersWithPositionsFromParent(GameObject.FindGameObjectWithTag("TowerParent"));
        foreach (var item in towersWithPositions) {
//            Debug.Log(item.Value.name);
        }
    }
    private void Start() {
        
        
    }
    
    
}
