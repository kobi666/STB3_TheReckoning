using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerTowersManager : MonoBehaviour
{
    public TowerController[] PlayerTowersByIndex = new TowerController[4];
    
    
    [ShowInInspector]
    public Dictionary<int, TowerController> ActiveTowers = new Dictionary<int, TowerController>();
}



