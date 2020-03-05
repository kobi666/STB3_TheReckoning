using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerTestScript : MonoBehaviour
{


    [SerializeField]
    public Dictionary<Vector2, TowerUtils.TowerPositionData> towersByDirections = new Dictionary<Vector2, TowerUtils.TowerPositionData>();
    // Start is called before the first frame update
    public DebugTowerPositionData[] TowersDebug = new DebugTowerPositionData[7];

    public GameObject TestTarget;
    int _index = 0;
    void Start()
    {
        towersByDirections = TowerUtils.TowersByCardinalDirections(gameObject, SelectorTest.instance.towersWithPositions);
        foreach (var item in towersByDirections)
        {
            if (item.Value != null) {
                item.Value.TowerGO.GetComponent<LineRenderer>().SetPosition(0, item.Value.TowerGO.transform.position);
                item.Value.TowerGO.GetComponent<LineRenderer>().SetPosition(1, transform.position);
            }
        }
    
        for(int i = 0 ; i < 8 ; i++) {
            TowersDebug[i].GO = towersByDirections[TowerUtils.DirectionsClockwise[i]].TowerGO;
            TowersDebug[i].Position = towersByDirections[TowerUtils.DirectionsClockwise[i]].TowerPosition;
            TowersDebug[i].Distance = towersByDirections[TowerUtils.DirectionsClockwise[i]].Distance;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        DebugText.D.SetText(TowerUtils.FindAngleBetweenTwoObjects(transform.position, TestTarget.transform.position).ToString());
    }
}
