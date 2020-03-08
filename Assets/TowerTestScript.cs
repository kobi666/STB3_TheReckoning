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
        // towersByDirections8 = TowerUtils.TowersByCardinalDirections8(gameObject, SelectorTest.instance.towersWithPositions);
        // foreach (var item in towersByDirections8)
        // {
        //     if (item.Value.TowerGO != null) {
        //         item.Value.TowerGO.GetComponent<LineRenderer>().SetPosition(0, item.Value.TowerGO.transform.position);
        //         item.Value.TowerGO.GetComponent<LineRenderer>().SetPosition(1, transform.position);
        //     }
        // }

        //towersByDirections4 = TowerUtils.TowersByCardinalDirections(gameObject, SelectorTest.instance.towersWithPositions, TowerUtils.Cardinal4);
        towersByDirections4 = TowerUtils.CardinalTowersNoAngles(gameObject, SelectorTest2.instance.towersWithPositions, TowerUtils.Cardinal4);
        // foreach (var item in towersByDirections8)
        // {
        //     if (item.Value.TowerGO != null) {
        //         item.Value.TowerGO.GetComponent<LineRenderer>().SetPosition(0, item.Value.TowerGO.transform.position);
        //         item.Value.TowerGO.GetComponent<LineRenderer>().SetPosition(1, transform.position);
        //     }
        // }
    
        // for(int i = 0 ; i < 8 ; i++) {
        //     TowersDebug[i].GO = towersByDirections8[TowerUtils.DirectionsClockwise8[i]].TowerGO;
        //     TowersDebug[i].Direction = TowerUtils.DirectionNamesClockWise8[i];
        //     TowersDebug[i].Position = towersByDirections8[TowerUtils.DirectionsClockwise8[i]].TowerPosition;
        //     TowersDebug[i].Distance = towersByDirections8[TowerUtils.DirectionsClockwise8[i]].Distance;
        // }

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
