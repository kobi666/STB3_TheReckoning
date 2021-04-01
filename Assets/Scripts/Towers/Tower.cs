using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    int componentCount = 0;

    public Dictionary<int,TowerComponent> TowerComponents = new Dictionary<int, TowerComponent>();

    public void AddTowerComponent(TowerComponent towerComponent) {
        TowerComponents.Add(componentCount, towerComponent);
        componentCount += 1;
    }


    private void Start() {
        foreach (var item in GetComponentsInChildren<TowerComponent>())
        {
            TowerComponents.Add(componentCount,item);
            componentCount += 1;
        }
    }
    
}
