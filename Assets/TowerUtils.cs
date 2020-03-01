using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerUtils : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<Vector2, GameObject> TowersWithPositionsFromParent (GameObject towerParent) {
        Dictionary<Vector2, GameObject> allTowersUnderParentObject = new Dictionary<Vector2, GameObject>();
        for (int i = 0 ; i <= towerParent.transform.childCount ; i++) {
            if (towerParent.transform.GetChild(i).CompareTag("Tower")){
                allTowersUnderParentObject.Add((Vector2)towerParent.transform.GetChild(i).transform.position, towerParent.transform.GetChild(i).gameObject);
            }
        }
        return allTowersUnderParentObject;
    }

    

    public class TowersByCardinalDirections {
        TowerAndPosition up;
        public TowerAndPosition UP {get => up;}
        TowerAndPosition up_right;
        public TowerAndPosition UP_RIGHT {get => up_right;}
        TowerAndPosition right;
        public TowerAndPosition RIGHT {get => right;}
        TowerAndPosition right_down;
        public TowerAndPosition RIGHT_DOWN {get => right_down;}
        TowerAndPosition down;
        public TowerAndPosition DOWN {get => down;}
        TowerAndPosition down_left;
        public TowerAndPosition DOWN_LEFT {get => down_left;}
        TowerAndPosition left;
        public TowerAndPosition LEFT {get => left;}
        TowerAndPosition up_left;
        public TowerAndPosition UP_LEFT {get => up_left;}
    }


}
