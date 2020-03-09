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
        
        towersByDirections4 = TowerUtils.CardinalTowersNoAngles(gameObject, SelectorTest2.instance.towersWithPositions, TowerUtils.Cardinal4);
        

        for(int i = 0 ; i < 4 ; i++) {
            TowersDebug[i].GO = towersByDirections4[TowerUtils.Cardinal4.directionsClockwise[i]].TowerGO;
            TowersDebug[i].Direction = TowerUtils.Cardinal4.directionNamesClockwise[i];
            TowersDebug[i].Position = towersByDirections4[TowerUtils.Cardinal4.directionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = towersByDirections4[TowerUtils.Cardinal4.directionsClockwise[i]].Distance;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //DebugText.D.SetText(TowerUtils.FindAngleBetweenTwoObjects(transform.position, TestTarget.transform.position).ToString());
    }
}
