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
public class TowerPositionData {
    [SerializeField]
    float distance;
    public float Distance {
        get => distance;
        set {
            distance = value;
        }
    }
    [SerializeField]
    GameObject towerGO;
    
    [SerializeField]
    Vector2 towerPosition;
    public GameObject TowerGO {
        get => towerGO;
        set {
            if (value == null) {
                //Debug.Log("Tower set to null");
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
    public TowerPositionData(GameObject _towerGO, float _distance) {
        TowerGO = _towerGO;
        if (_towerGO != null) {
        TowerPosition = (Vector2)_towerGO.transform.position;
        }
        Distance = _distance;
    }
}

    public static TowerPositionData FindNearestTowerToCardinalPoint(Dictionary<Vector2,GameObject> allTowers, Vector2 position, Vector2 direction) {
        TowerPositionData nearestTower = null;
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
                    nearestTower = new TowerPositionData(tower.Value, t);
                }
            }
        return nearestTower;
    }

    public static Vector2[] DirectionsClockwise = { new Vector2(0,1), new Vector2(1,1),new Vector2(1,0),new Vector2(1,-1),new Vector2(0,-1),new Vector2(-1,-1),new Vector2(-1,0),new Vector2(-1,1)};
    public static float[] AnglesClockwise = {90,45,360,315,270,225,180,135};
    public static string[] DirectionNamesClockWise = {"UP","UP_RIGHT","RIGHT","DOWN_RIGHT","DOWN","DOWN_LEFT","LEFT","UP_LEFT"};
    
       
    


    [SerializeField]
    public static Dictionary<Vector2, TowerPositionData> TowersByCardinalDirections(GameObject self, Dictionary<Vector2, GameObject> allTowers) {
        Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
        string name = self.name;
        Vector2 myPosition = self.transform.position;
        for (int i = 0 ; i < 8 ; i++) {
            dict.Add(DirectionsClockwise[i], new TowerPositionData(null, 999f));
        }
        foreach (var item in allTowers)
        {
            
            if (item.Value.name == name) {
                continue;
            }
            if (item.Value == null) {
                continue;
            }
            float distance = Vector2.Distance(myPosition, item.Key);
            float angle = FindAngleBetweenTwoObjects(myPosition, item.Key);
            for (int i = 0 ; i < 8 ; i++) {
                if (IsAngleIn45Range(AnglesClockwise[i], angle)) {
                    if(dict[DirectionsClockwise[i]].Distance > distance) {
                        dict[DirectionsClockwise[i]] = new TowerPositionData(item.Value, distance);
                    }
                }
            }
            
        }
        return dict;
    }   



    

    // public class TTowersByCardinalDirections : IEnumerable<TowerPositionData> {

    //     public Dictionary<Vector2, TowerPositionData> towerDict = new Dictionary<Vector2, TowerPositionData>();

    //     List<TowerPositionData> mylist = new List<TowerPositionData>();
        
    //     public TowerPositionData this[int index] {
    //         get { return mylist[index];}
    //         set { mylist[index] = value;}
    //     }
    //     public IEnumerator<TowerPositionData> GetEnumerator()
    //     {
    //         return mylist.GetEnumerator();
    //     }

    //     IEnumerator IEnumerable.GetEnumerator()
    //     {
    //         return this.GetEnumerator();
    //     }

    //     [SerializeField]
    //     TowerPositionData up;
    //     public TowerPositionData UP {get => up;}
    //     [SerializeField]
    //     TowerPositionData up_right;
    //     public TowerPositionData UP_RIGHT {get => up_right;}
    //     [SerializeField]
    //     TowerPositionData right;
    //     public TowerPositionData RIGHT {get => right;}
    //     [SerializeField]
    //     TowerPositionData down_right;
    //     public TowerPositionData DOWN_RIGHT {get => down_right;}
    //     [SerializeField]
    //     TowerPositionData down;
    //     public TowerPositionData DOWN {get => down;}
    //     [SerializeField]
    //     TowerPositionData down_left;
    //     public TowerPositionData DOWN_LEFT {get => down_left;}
    //     [SerializeField]
    //     TowerPositionData left;
    //     public TowerPositionData LEFT {get => left;}
    //     [SerializeField]
    //     TowerPositionData up_left;
    //     public TowerPositionData UP_LEFT {get => up_left;}

    //     public TTowersByCardinalDirections(Dictionary<Vector2, GameObject> allTowers, GameObject self) {

    //         for (int i = 0 ; i <= 8 ; i++) {
    //         mylist.Add(null);
    //         }
    //         Vector2 myPosition = (Vector2)self.transform.position;
    //         string name = self.name;
    //         float u_d = 999;
    //         float ur_d = 999;
    //         float r_d = 999;
    //         float rd_d = 999;
    //         float d_d = 999;
    //         float dl_d = 999;
    //         float l_d = 999;
    //         float lu_d = 999;
    //         foreach (var item in allTowers)
    //         {
    //             if (item.Value.name == name) {
    //                 continue;
    //             }
    //             if (item.Value == null) {
    //                 continue;
    //             }
    //             float distance = Vector2.Distance(myPosition, item.Key);
    //             float angle = FindAngleBetweenTwoObjects(myPosition, item.Key);
    //             if (IsAngleIn45Range(180, angle)) {
    //               if (distance < l_d) {
    //                   l_d = distance;
                      
    //                   left = new TowerPositionData(item.Value, l_d);
    //                   mylist[0] = (new TowerPositionData(item.Value));  
    //                   towerDict.Add(new Vector2(-1,0), left);
    //               }
    //             }
    //             if (IsAngleIn45Range(135, angle)) {
    //                 if (distance < lu_d) {
    //                     lu_d = distance;
    //                     up_left = new TowerPositionData(item.Value);
    //                     mylist[1] =(new TowerPositionData(item.Value));
    //                     towerDict.Add(new Vector2(-1, 1), up_left);
    //                 }
    //             }
    //             if (IsAngleIn45Range(90, angle)) {
    //                 if (distance < u_d) {
    //                     u_d = distance;
    //                     up = new TowerPositionData(item.Value);
    //                     mylist[2] =(new TowerPositionData(item.Value));
    //                     towerDict.Add(new Vector2(0, 1), up);
    //                 }
    //             }
    //             if (IsAngleIn45Range(45, angle)) {
    //                 if (distance < ur_d) {
    //                     ur_d = distance;
    //                     up_right = new TowerPositionData(item.Value);
    //                     mylist[3] =(new TowerPositionData(item.Value));
    //                     towerDict.Add(new Vector2(1, 1), up_right);
    //                 }
    //             }
    //             if (IsAngleIn45Range(360, angle)) {
    //                 if (distance < r_d) {
    //                     r_d = distance;
    //                     right = new TowerPositionData(item.Value);
    //                     mylist[4] =(new TowerPositionData(item.Value));
    //                     towerDict.Add(new Vector2(1, 0), right);
    //                 }
    //             }
    //             if (IsAngleIn45Range(315, angle)) {
    //                 if (distance < rd_d) {
    //                     rd_d = distance;
    //                     down_right = new TowerPositionData(item.Value);
    //                     mylist[5] =(new TowerPositionData(item.Value));
    //                     towerDict.Add(new Vector2(1, -1), down_right);
    //                 }
    //             }
    //             if (IsAngleIn45Range(270, angle)) {
    //                 if (distance < d_d) {
    //                     d_d = distance;
    //                     down = new TowerPositionData(item.Value);
    //                     mylist[6] =(new TowerPositionData(item.Value));
    //                     towerDict.Add(new Vector2(0, -1), down);
    //                 }
    //             }
    //             if (IsAngleIn45Range(215, angle)) {
    //                 if (distance < dl_d) {
    //                     dl_d = distance;
    //                     down_left = new TowerPositionData(item.Value);
    //                     mylist[7] =(new TowerPositionData(item.Value));
    //                     towerDict.Add(new Vector2(-1, -1), down_left);
    //                 }
    //             }
    //         }
    //     }
    // }

    public static TowerPositionData GetTowerFromNormalizedDirection(Vector2 Nd, Dictionary<Vector2, TowerPositionData> _towers) {
        TowerPositionData tower = new TowerPositionData(null, 0.0f);
        
        return tower;
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
