using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArsenal : MonoBehaviour
{
    public TowerItem EmptyTowerSlot;
    public TowerItem TestTower1;
    public TowerItem TestTower2;
    public TowerItem TestTower3;
    public TowerItem TestTower4;

    public static TowerArsenal arsenal;
    private void Awake() {
        arsenal = this;
    }
    private void Start() {
        
    }
}
