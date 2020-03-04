using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerUtils : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<Vector2, GameObject> TowersWithPositionsFromParent (GameObject towerParent) {
        Dictionary<Vector2, GameObject> allTowersUnderParentObject = new Dictionary<Vector2, GameObject>();
        for (int i = 0 ; i < towerParent.transform.childCount ; i++) {
            //if (towerParent.transform.GetChild(i).CompareTag("Tower")){
                if (allTowersUnderParentObject.ContainsKey((Vector2)towerParent.transform.GetChild(i).transform.position)) {
                    continue;
                }
                allTowersUnderParentObject.Add((Vector2)towerParent.transform.GetChild(i).transform.position, towerParent.transform.GetChild(i).gameObject);
            //}
        }
        return allTowersUnderParentObject;
    }

[System.Serializable]
public class TowerAndPosition {
    [SerializeField]
    GameObject towerGO;
    
    [SerializeField]
    Vector2 towerPosition;
    public GameObject TowerGO {
        get => towerGO;
        set {
            if (value == null) {
                Debug.Log("Tower set to null");
            }
            towerGO = value;
            }
    
        }
    public Vector2 TowerPosition {
        get => towerPosition;
        set {
            towerPosition = value;
        }
    }
    public TowerAndPosition(GameObject _towerGO) {
        TowerGO = _towerGO;
        TowerPosition = (Vector2)_towerGO.transform.position;
    }
}

    public static TowerAndPosition FindNearestTowerToCardinalPoint(Dictionary<Vector2,GameObject> allTowers, Vector2 position, Vector2 direction) {
        TowerAndPosition nearestTower = null;
        Dictionary<Vector2,GameObject> alltowersButMyself = new Dictionary<Vector2, GameObject>();
        alltowersButMyself = allTowers;
        Vector2 newPosition = position + direction;
        if (alltowersButMyself.ContainsKey(position)) {
            alltowersButMyself.Remove(position);
        }
        
        float distance = 999;
        foreach (KeyValuePair<Vector2,GameObject> tower in allTowers) {
            float t = Vector2.Distance(newPosition, (Vector2)tower.Value.transform.position) ;
                if (t < distance) {
                    distance = t;
                    nearestTower = new TowerAndPosition(tower.Value);
                }
            }
        return nearestTower;
    }
    

    
[System.Serializable]
    public class FindTowersByCardinalDirections : IEnumerable<TowerAndPosition> {

        List<TowerAndPosition> mylist = new List<TowerAndPosition>();
        
        public TowerAndPosition this[int index] {
            get { return mylist[index];}
            set { mylist[index] = value;}
        }
        public IEnumerator<TowerAndPosition> GetEnumerator()
        {
            return mylist.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [SerializeField]
        TowerAndPosition up;
        public TowerAndPosition UP {get => up;}
        [SerializeField]
        TowerAndPosition up_right;
        public TowerAndPosition UP_RIGHT {get => up_right;}
        [SerializeField]
        TowerAndPosition right;
        public TowerAndPosition RIGHT {get => right;}
        [SerializeField]
        TowerAndPosition down_right;
        public TowerAndPosition DOWN_RIGHT {get => down_right;}
        [SerializeField]
        TowerAndPosition down;
        public TowerAndPosition DOWN {get => down;}
        [SerializeField]
        TowerAndPosition down_left;
        public TowerAndPosition DOWN_LEFT {get => down_left;}
        [SerializeField]
        TowerAndPosition left;
        public TowerAndPosition LEFT {get => left;}
        [SerializeField]
        TowerAndPosition up_left;
        public TowerAndPosition UP_LEFT {get => up_left;}

        public FindTowersByCardinalDirections(Dictionary<Vector2, GameObject> allTowers, GameObject self) {
            for (int i = 0 ; i <= 8 ; i++) {
            mylist.Add(null);
            }
            Vector2 myPosition = (Vector2)self.transform.position;
            string name = self.name;
            float u_d = 999;
            float ur_d = 999;
            float r_d = 999;
            float rd_d = 999;
            float d_d = 999;
            float dl_d = 999;
            float l_d = 999;
            float lu_d = 999;
            foreach (var item in allTowers)
            {
                if (item.Value.name == name) {
                    continue;
                }
                float distance = Vector2.Distance(myPosition, item.Key);
                float angle = FindAngleBetweenTwoObjects(myPosition, item.Key);
                if (IsAngleIn45Range(180, angle)) {
                  if (distance < l_d) {
                      l_d = distance;
                      left = new TowerAndPosition(item.Value);
                      mylist[0] = (new TowerAndPosition(item.Value));  
                  }
                }
                if (IsAngleIn45Range(135, angle)) {
                    if (distance < lu_d) {
                        lu_d = distance;
                        up_left = new TowerAndPosition(item.Value);
                        mylist[1] =(new TowerAndPosition(item.Value));
                    }
                }
                if (IsAngleIn45Range(90, angle)) {
                    if (distance < u_d) {
                        u_d = distance;
                        up = new TowerAndPosition(item.Value);
                        mylist[2] =(new TowerAndPosition(item.Value));
                    }
                }
                if (IsAngleIn45Range(45, angle)) {
                    if (distance < ur_d) {
                        ur_d = distance;
                        up_right = new TowerAndPosition(item.Value);
                        mylist[3] =(new TowerAndPosition(item.Value));
                    }
                }
                if (IsAngleIn45Range(360, angle)) {
                    if (distance < r_d) {
                        r_d = distance;
                        right = new TowerAndPosition(item.Value);
                        mylist[4] =(new TowerAndPosition(item.Value));
                    }
                }
                if (IsAngleIn45Range(315, angle)) {
                    if (distance < rd_d) {
                        rd_d = distance;
                        down_right = new TowerAndPosition(item.Value);
                        mylist[5] =(new TowerAndPosition(item.Value));
                    }
                }
                if (IsAngleIn45Range(270, angle)) {
                    if (distance < d_d) {
                        d_d = distance;
                        down = new TowerAndPosition(item.Value);
                        mylist[6] =(new TowerAndPosition(item.Value));
                    }
                }
                if (IsAngleIn45Range(215, angle)) {
                    if (distance < dl_d) {
                        dl_d = distance;
                        down_left = new TowerAndPosition(item.Value);
                        mylist[7] =(new TowerAndPosition(item.Value));
                    }
                }
            }
        }
    }

    public static bool IsAngleIn45Range(float angleToCheck, float angleToObject) {
        bool in360Range = false;
        float AngleRange =  22.5f + 7.5f;       //350
        float delta;
        float min = angleToCheck - AngleRange;
        float max = angleToCheck + AngleRange;
        if (min < 0) {
            delta = 0 + min;
            in360Range = true;
            min = delta;
        }
        if (max > 360) {
            max = max - 360;
            in360Range = true;
        }

        

        if (in360Range) {
            if (angleToObject <= max) {
                    return true;
            }
            if (angleToObject >= min) {
                return true;
            }
        }


        


        

        if (angleToObject >= min && angleToObject <= max) {
            return true;
        }

        else {
            return false;
            }
    }

    


    public static float FindAngleBetweenTwoObjects(Vector2 myPosition, Vector2 targetPosition){
    Vector2 directon = targetPosition - myPosition;
     float value = (float)((Mathf.Atan2(directon.y, directon.x) / Math.PI) * 180f);
     if(value < 0) value += 360f;
     return value;
 }



}
