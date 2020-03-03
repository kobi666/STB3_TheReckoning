using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerTestScript : MonoBehaviour
{
    [SerializeField]
    public TowerUtils.FindTowersByCardinalDirections towersByDirections;
    // Start is called before the first frame update

    public GameObject TestTarget;
    int _index = 0;
    void Start()
    {
        towersByDirections = new TowerUtils.FindTowersByCardinalDirections(SelectorTest.instance.towersWithPositions, gameObject);
        foreach (var item in towersByDirections)
        {
            if (item != null) {
                item.TowerGO.GetComponent<LineRenderer>().SetPosition(0, item.TowerGO.transform.position);
                item.TowerGO.GetComponent<LineRenderer>().SetPosition(1, transform.position);
            }
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        DebugText.D.SetText(TowerUtils.FindAngleBetweenTwoObjects(transform.position, TestTarget.transform.position).ToString());
    }
}
