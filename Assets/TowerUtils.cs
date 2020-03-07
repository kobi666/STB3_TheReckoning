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
    public int CardinalIndex;
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

    public int ClockWiseIndex;
    

    public int CounterClockwiseIndex;
    

    

    public TowerPositionData(GameObject _towerGO, float _distance, int cardinalIndex) {
        TowerGO = _towerGO;
        if (_towerGO != null) {
        TowerPosition = (Vector2)_towerGO.transform.position;
        }
        Distance = _distance;
    }
}

    // public static TowerPositionData FindNearestTowerToCardinalPoint(Dictionary<Vector2,GameObject> allTowers, Vector2 position, Vector2 direction) {
    //     TowerPositionData nearestTower = null;
    //     Dictionary<Vector2,GameObject> alltowersButMyself = new Dictionary<Vector2, GameObject>();
    //     alltowersButMyself = allTowers;
    //     Vector2 newPosition = position + direction;
    //     if (alltowersButMyself.ContainsKey(position)) {
    //         alltowersButMyself.Remove(position);
    //     }
        
    //     float distance = 999;
    //     foreach (KeyValuePair<Vector2,GameObject> tower in allTowers) {
    //         float t = Vector2.Distance(newPosition, (Vector2)tower.Value.transform.position) ;
    //             if (t < distance) {
    //                 distance = t;
    //                 nearestTower = new TowerPositionData(tower.Value, t);
    //             }
    //         }
    //     return nearestTower;
    // }

    public static Vector2[] DirectionsClockwise8 = { new Vector2(0,1), new Vector2(1,1),new Vector2(1,0),new Vector2(1,-1),new Vector2(0,-1),new Vector2(-1,-1),new Vector2(-1,0),new Vector2(-1,1)};
    public static Vector2[] DirectionsClockwise4 = { new Vector2(0,1),new Vector2(1,0),new Vector2(0,-1),new Vector2(-1,0)};
    public static float[] AnglesClockwise8 = {90,45,360,315,270,225,180,135};
    public static float[] AnglesClockwise4 = {90,360,270,180};
    public static string[] DirectionNamesClockWise8 = {"UP","UP_RIGHT","RIGHT","DOWN_RIGHT","DOWN","DOWN_LEFT","LEFT","UP_LEFT"};
    public static string[] DirectionNamesClockWise4 = {"UP","RIGHT","DOWN","LEFT"};

    public class CardinalSet {
        public Vector2[] directionsClockwise;
        public float[] anglesClockwise;
        public string[] directionNamesClockwise;
        public int length {
            get => directionsClockwise.Length;
        }
        public CardinalSet (Vector2[] dirs, float[] angles, string[] dirNames) {
            directionsClockwise = dirs;
            anglesClockwise = angles;
            directionNamesClockwise = dirNames;
        }
    }

    public static CardinalSet Cardinal4 = new CardinalSet(DirectionsClockwise4, AnglesClockwise4, DirectionNamesClockWise4);
    public static CardinalSet Cardinal8 = new CardinalSet(DirectionsClockwise8, AnglesClockwise8, DirectionNamesClockWise8);
    
       
    


    [SerializeField]
    public static Dictionary<Vector2, TowerPositionData> TowersByCardinalDirections8(GameObject self, Dictionary<Vector2, GameObject> allTowers) {
        Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
        string name = self.name;
        Vector2 myPosition = self.transform.position;
        for (int i = 0 ; i < 8 ; i++) {
            dict.Add(DirectionsClockwise8[i], new TowerPositionData(null, 999f, i));
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
                if (IsAngleIn45Range(AnglesClockwise8[i], angle)) {
                    if(dict[DirectionsClockwise8[i]].Distance > distance) {
                        dict[DirectionsClockwise8[i]] = new TowerPositionData(item.Value, distance, i);
                    }
                }
            }
            for (int i = 0 ; i < 8 ; i++) {
                if (i == 0) {
                    dict[DirectionsClockwise8[i]].ClockWiseIndex = i+1;
                    dict[DirectionsClockwise8[i]].CounterClockwiseIndex = 7;
                    continue;
                }
                if (i == 7) {
                    dict[DirectionsClockwise8[i]].ClockWiseIndex = 0;
                    dict[DirectionsClockwise8[i]].CounterClockwiseIndex = i-1;
                    continue;
                }
                else {
                    dict[DirectionsClockwise8[i]].ClockWiseIndex = i+1;
                    dict[DirectionsClockwise8[i]].CounterClockwiseIndex = i-1;
                }
            }
            
        }
        return dict;
    }

    public static Dictionary<Vector2, TowerPositionData> TowersByCardinalDirections(GameObject self, Dictionary<Vector2, GameObject> allTowers, CardinalSet cardinalSet) {
        Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
        string name = self.name;
        Vector2 myPosition = self.transform.position;
        for (int i = 0 ; i < cardinalSet.length ; i++) {
            dict.Add(cardinalSet.directionsClockwise[i], new TowerPositionData(null, 999f, i));
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
            for (int i = 0 ; i < cardinalSet.length ; i++) {
                if (IsAngleInRange(cardinalSet.anglesClockwise[i], angle)) {
                    if(dict[cardinalSet.directionsClockwise[i]].Distance > distance) {
                        dict[cardinalSet.directionsClockwise[i]] = new TowerPositionData(item.Value, distance, i);
                    }
                }
            }
            for (int i = 0 ; i < cardinalSet.length ; i++) {
                if (i == 0) {
                    dict[cardinalSet.directionsClockwise[i]].ClockWiseIndex = i+1;
                    dict[cardinalSet.directionsClockwise[i]].CounterClockwiseIndex = cardinalSet.length;
                    continue;
                }
                if (i == cardinalSet.length) {
                    dict[cardinalSet.directionsClockwise[i]].ClockWiseIndex = 0;
                    dict[cardinalSet.directionsClockwise[i]].CounterClockwiseIndex = i-1;
                    continue;
                }
                else {
                    dict[cardinalSet.directionsClockwise[i]].ClockWiseIndex = i+1;
                    dict[cardinalSet.directionsClockwise[i]].CounterClockwiseIndex = i-1;
                }
            }
            
        }
        return dict;
    }

    public static bool IsAngleIn45Range(float angleToCheck, float angleToObject) {
        bool in360Range = false;
        float AngleRange =  17.5f;       //350
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

    public static bool IsAngleInRange(float angleToCheck, float angleToObject) {
        bool in360Range = false;
        float AngleRange =  angleToCheck / 2.0f;
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
