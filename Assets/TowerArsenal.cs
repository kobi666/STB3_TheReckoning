using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArsenal : MonoBehaviour
{
    public TowerItem TestTower1;
    public TowerItem TestTower2;
    public TowerItem TestTower3;
    public TowerItem TestTower4;

    public static TowerArsenal arsenal;

    private void Start() {
        arsenal = this;
    }
}
