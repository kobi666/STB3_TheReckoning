using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Tower : MonoBehaviour
{
    // Start is called before the first frame update
    public Dictionary<string, TowerComponent> Components = new Dictionary<string, TowerComponent>();
    public EnemyTargetBank TargetBank;
    private void Awake() {
        
    }
}
