using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerTestScript : MonoBehaviour
{


    [SerializeField]
    public Dictionary<Vector2, TowerUtils.TowerPositionData> towersByDirections8 = new Dictionary<Vector2, TowerUtils.TowerPositionData>();
    public Dictionary<Vector2, TowerUtils.TowerPositionData> towersByDirections4 = new Dictionary<Vector2, TowerUtils.TowerPositionData>();
    // Start is called before the first frame update
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[8];

    public GameObject TestTarget;
    int _index = 0;
    void Start()
    {
        
        towersByDirections8 = TowerUtils.CardinalTowersNoAnglesLoop(gameObject, SelectorTest2.instance.towersWithPositions, TowerUtils.Cardinal8);
        

        for(int i = 0 ; i < 8 ; i++) {
            TowersDebug[i].GO = towersByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerGO;
            TowersDebug[i].Direction = TowerUtils.Cardinal8.directionNamesClockwise[i];
            TowersDebug[i].Position = towersByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = towersByDirections8[TowerUtils.Cardinal8.directionsClockwise[i]].Distance;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //DebugText.D.SetText(TowerUtils.FindAngleBetweenTwoObjects(transform.position, TestTarget.transform.position).ToString());
    }
}
