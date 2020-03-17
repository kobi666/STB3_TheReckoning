using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerUtils : MonoBehaviour
{

    public static TowerSlotActions DefaultSlotActions = new TowerSlotActions(null,null,null,null);

    public static void PlaceTowerInSlot(TowerItem tower, GameObject TargetSlotObject, GameObject towerSlotParent) {
        TargetSlotObject = Instantiate(tower.TowerPrefab,towerSlotParent.transform.position, Quaternion.identity, towerSlotParent.transform);
        TargetSlotObject.name = (tower.TowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
    }

    public static GameObject PlaceTowerInSlotGO(TowerItem tower, GameObject towerSlotParent) {
        GameObject newGO = GameObject.Instantiate(tower.TowerPrefab,towerSlotParent.transform.position, Quaternion.identity, towerSlotParent.transform);
        newGO.name = (tower.TowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
        return newGO;
    }

    public static void PlaceTowerObjectAndDestroyOldObject(GameObject newTowerPrefab, GameObject towerObject, GameObject oldObjectForDestruction, GameObject towerSlot) {
        oldObjectForDestruction = towerObject;
        towerObject = Instantiate(newTowerPrefab, towerSlot.transform.position, Quaternion.identity, towerSlot.transform);
        towerObject.name = (newTowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
        Destroy(oldObjectForDestruction);
    }


    
    public static Vector2 GetCardinalDirectionFromAxis(Vector2 movementInput) {
        //Debug.Log(movementInput);
        Vector2 NormalizedVector = Vector2.zero;
        if (movementInput.x > 0.1f) {
            NormalizedVector.x = 1;
        }
        if (movementInput.x < -0.1f) {
            NormalizedVector.x = -1;
        }
        if (movementInput.y > 0.1f) {
            NormalizedVector.y = 1;
        }
        if (movementInput.y < -0.1f) {
            NormalizedVector.y = -1;
        }
        return NormalizedVector;
    }
    // Start is called before the first frame update
    public static Dictionary<Vector2, GameObject> TowerSlotsWithPositionsFromParent (GameObject towerParent) {
        Dictionary<Vector2, GameObject> allTowersUnderParentObject = new Dictionary<Vector2, GameObject>();
        for (int i = 0 ; i < towerParent.transform.childCount ; i++) {
            
                if (allTowersUnderParentObject.ContainsKey((Vector2)towerParent.transform.GetChild(i).transform.position)) {
                    continue;
                }
                if (!towerParent.transform.GetChild(i).CompareTag("TowerSlot")) {
                    continue;
                }
                if (towerParent.transform.GetChild(i).gameObject.activeSelf == false) {
                    continue;
                }
                allTowersUnderParentObject.Add((Vector2)towerParent.transform.GetChild(i).transform.position, towerParent.transform.GetChild(i).gameObject);
            
        }
        return allTowersUnderParentObject;
    }

public static Dictionary<Vector2, TowerPositionData> CardinalTowersNoAnglesLoop(GameObject self, Dictionary<Vector2, GameObject> allTowers, CardinalSet cardinalSet) {
    Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
    Vector2 selfPosition = self.transform.position;
    float towerDiscoveryRange = StaticObjects.instance.TowerSize;
    float SecondTowerDiscoveryRange = SelectorTest2.instance.SecondDiscoveryRange;
    float RangeMultiplier = SelectorTest2.instance.SecondDiscoveryRangeMultiplier;
    
    for (int i =0 ; i < cardinalSet.length ; i++ ) {
        dict.Add(cardinalSet.directionsClockwise[i], new TowerPositionData(null, 999f, i));
    }
    foreach (var item in allTowers)
    {
            if (item.Value.name == self.name || item.Value == null) {
                continue;
            }
            
            TowerPositionQuery tq = new TowerPositionQuery(selfPosition, item.Key, towerDiscoveryRange);
            for(int i = 0 ; i < cardinalSet.length ; i++) {
                if (cardinalSet.discoveryConditionsClockwise[i](tq)) {
                    float d = Vector2.Distance(selfPosition, item.Key);
                    if (dict[cardinalSet.directionsClockwise[i]].Distance > d) {
                        dict[cardinalSet.directionsClockwise[i]] = new TowerPositionData(item.Value, d, i);
                    }
                }
            }
            
    }
    // Second check with extended range for vertical/horizontal towers to make sure we are catching horizontal towers with priority to shorter vertical/horizontal range area.
    foreach (var item in allTowers)
    {
            if (item.Value.name == self.name || item.Value == null) {
                continue;
            }
            //dict[DirectionsClockwise4[0]] = new TowerPositionData(item.Value, Vector2.Distance(item.Key, selfPosition + towerDiscoveryRangeY), 0);
            //Get UP tower
            TowerPositionQuery tq = new TowerPositionQuery(selfPosition, item.Key, SecondTowerDiscoveryRange);
            for(int i = 0 ; i < cardinalSet.length ; i+=2) {
                if(dict[cardinalSet.directionsClockwise[i]].TowerSlotGo == null) {
                    if (cardinalSet.discoveryConditionsClockwise[i](tq)) {
                        float d = Vector2.Distance(selfPosition, item.Key);
                        if (dict[cardinalSet.directionsClockwise[i]].Distance > d) {
                            dict[cardinalSet.directionsClockwise[i]] = new TowerPositionData(item.Value, d, i);
                        }
                    }
                }
            }
            
    }
    foreach (var item in allTowers)
    {
            if (item.Value.name == self.name || item.Value == null) {
                continue;
            }
            //dict[DirectionsClockwise4[0]] = new TowerPositionData(item.Value, Vector2.Distance(item.Key, selfPosition + towerDiscoveryRangeY), 0);
            //Get UP tower
            TowerPositionQuery tq = new TowerPositionQuery(selfPosition, item.Key, SecondTowerDiscoveryRange * RangeMultiplier);
            for(int i = 0 ; i < cardinalSet.length ; i+=2) {
                if(dict[cardinalSet.directionsClockwise[i]].TowerSlotGo == null) {
                    if (cardinalSet.discoveryConditionsClockwise[i](tq)) {
                        float d = Vector2.Distance(selfPosition, item.Key);
                        if (dict[cardinalSet.directionsClockwise[i]].Distance > d) {
                            dict[cardinalSet.directionsClockwise[i]] = new TowerPositionData(item.Value, d, i);
                        }
                    }
                }
            }
            
    } 
    return dict;
    }
    


public static Dictionary<Vector2, TowerPositionData> CardinalTowersNoAngles(GameObject self, Dictionary<Vector2, GameObject> allTowers, CardinalSet cardinalSet) {
    Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
    Vector2 selfPosition = self.transform.position;
    float towerDiscoveryRange = 1f;
    Vector2 towerDiscoveryRangeY = new Vector2(0, towerDiscoveryRange);
    Vector2 towerDiscoveryRangeX = new Vector2(towerDiscoveryRange, 0);
    for (int i =0 ; i < cardinalSet.length ; i++ ) {
        dict.Add(cardinalSet.directionsClockwise[i], new TowerPositionData(null, 999f, i));
    }
    foreach (var item in allTowers)
    {
        if (item.Value.name == self.name || item.Value == null) {
            continue;
        }
        //Get UP tower
        if (item.Key.y >= selfPosition.y + towerDiscoveryRange) {
            if (FindIfTowerInStraightPositionRangeXorY(selfPosition.x, item.Key.x, towerDiscoveryRange)) {
                if (dict[DirectionsClockwise4[0]].Distance > Vector2.Distance(item.Key, selfPosition)) {
                    dict[DirectionsClockwise4[0]] = new TowerPositionData(item.Value, Vector2.Distance(item.Key, selfPosition + towerDiscoveryRangeY), 0);
                }
            }
        }
        //Get RIGHT tower
        if (item.Key.x >= selfPosition.x + towerDiscoveryRange) {
            if (FindIfTowerInStraightPositionRangeXorY(selfPosition.y, item.Key.y, towerDiscoveryRange)) {
                if (dict[DirectionsClockwise4[1]].Distance > Vector2.Distance(item.Key, selfPosition + towerDiscoveryRangeX)) {
                    dict[DirectionsClockwise4[1]] = new TowerPositionData(item.Value, Vector2.Distance(item.Key, selfPosition + towerDiscoveryRangeX), 1);
                }
            }
        }
        //get DOWN tower
        if (item.Key.y <= selfPosition.y - towerDiscoveryRange) {
            if (FindIfTowerInStraightPositionRangeXorY(selfPosition.x, item.Key.x, towerDiscoveryRange)) {
                if (dict[DirectionsClockwise4[2]].Distance > Vector2.Distance(item.Key, selfPosition)) {
                    dict[DirectionsClockwise4[2]] = new TowerPositionData(item.Value, Vector2.Distance(item.Key, selfPosition - towerDiscoveryRangeY), 2);
                }
            }
        }
        //Get LEFT tower
        if (item.Key.x <= selfPosition.x - towerDiscoveryRange) {
            if (FindIfTowerInStraightPositionRangeXorY(selfPosition.y, item.Key.y, towerDiscoveryRange)) {
                if (dict[DirectionsClockwise4[3]].Distance > Vector2.Distance(item.Key, selfPosition)) {
                    dict[DirectionsClockwise4[3]] = new TowerPositionData(item.Value, Vector2.Distance(item.Key, selfPosition - towerDiscoveryRangeX), 3);
                }
            }
        }
       
        

        
        
    }
    return dict;
}

public static bool FindIfTowerInStraightPositionRangeXorY(float myPosXorY, float targetPosXorY, float posRange) {
    float max = myPosXorY + posRange;
    float min = myPosXorY - posRange;
    float halfTowerSize = posRange;
    if (targetPosXorY - halfTowerSize >= min) {
        if(targetPosXorY - halfTowerSize <= max) {
            return true;
        }
    }
    if (targetPosXorY + halfTowerSize >= min) {
        if(targetPosXorY + halfTowerSize <= max) {
            return true;
        }
    }
    return false;
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
    GameObject towerSlotGO;
    
    [SerializeField]
    Vector2 towerPosition;
    public GameObject TowerSlotGo {
        get => towerSlotGO;
        set {
            if (value == null) {
                //Debug.Log("Tower set to null");
            }
            towerSlotGO = value;
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
        TowerSlotGo = _towerGO;
        if (_towerGO != null) {
        TowerPosition = (Vector2)_towerGO.transform.position;
        }
        Distance = _distance;
    }
}

    public static Vector2[] DirectionsClockwise8 = { new Vector2(0,1), new Vector2(1,1),new Vector2(1,0),new Vector2(1,-1),new Vector2(0,-1),new Vector2(-1,-1),new Vector2(-1,0),new Vector2(-1,1)};
    public static Vector2[] DirectionsClockwise4 = { new Vector2(0,1),new Vector2(1,0),new Vector2(0,-1),new Vector2(-1,0)};
    public static float[] AnglesClockwise8 = {90,45,360,315,270,225,180,135};
    public static float[] AnglesClockwise4 = {90,360,270,180};
    public static string[] DirectionNamesClockWise8 = {"UP","UP_RIGHT","RIGHT","DOWN_RIGHT","DOWN","DOWN_LEFT","LEFT","UP_LEFT"};
    public static string[] DirectionNamesClockWise4 = {"UP","RIGHT","DOWN","LEFT"};
    public static Predicate<TowerPositionQuery>[] DiscoveryConditions8 = {GetUpTower, GetUpRightTower,GetRightTower,GetDownRightTower,GetDownTower,GetDownLeftTower,GetLeftTower,GetUpLeftTower};
    

    public class CardinalSet {
        public Vector2[] directionsClockwise;
        public float[] anglesClockwise;
        public string[] directionNamesClockwise;
        public Predicate<TowerPositionQuery>[] discoveryConditionsClockwise;
        public int length {
            get => directionsClockwise.Length;
        }
        public CardinalSet (Vector2[] dirs, float[] angles, string[] dirNames, Predicate<TowerPositionQuery>[] discoveryConditions) {
            directionsClockwise = dirs;
            anglesClockwise = angles;
            directionNamesClockwise = dirNames;
            discoveryConditionsClockwise = discoveryConditions;

        }
    }

    public static CardinalSet Cardinal4 = new CardinalSet(DirectionsClockwise4, AnglesClockwise4, DirectionNamesClockWise4, DiscoveryConditions8);
    public static CardinalSet Cardinal8 = new CardinalSet(DirectionsClockwise8, AnglesClockwise8, DirectionNamesClockWise8, DiscoveryConditions8);
    
       
    


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

    public static bool GetUpTower(TowerPositionQuery tq) {
        if (tq.TargetTower.y > tq.ThisTower.y + (tq.Assistingfloat1 / 2)) {
            return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.x, tq.TargetTower.x, tq.Assistingfloat1);
        }
        return false;
    }
    public static bool GetUpRightTower(TowerPositionQuery tq) {
        Vector2 pos = new Vector2(tq.ThisTower.x + tq.Assistingfloat1, tq.ThisTower.y + tq.Assistingfloat1);
        if (tq.TargetTower.y > pos.y) {
            if (tq.TargetTower.x > pos.x) {
                return true;
            }
        }
        return false;
    }

    public static bool GetRightTower(TowerPositionQuery tq) {
        if (tq.TargetTower.x > tq.ThisTower.x + (tq.Assistingfloat1 / 2)) {
        return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.y, tq.TargetTower.y, tq.Assistingfloat1);
        }
        return false;
    }

    public static bool GetDownRightTower(TowerPositionQuery tq) {
        Vector2 pos = new Vector2(tq.ThisTower.x + tq.Assistingfloat1, tq.ThisTower.y - tq.Assistingfloat1);
        if (tq.TargetTower.y < pos.y) {
            if (tq.TargetTower.x > pos.x) {
                return true;
            }
        }
        return false;
    }

    public static bool GetDownTower(TowerPositionQuery tq) {
        if (tq.TargetTower.y < tq.ThisTower.y - (tq.Assistingfloat1 / 2)) {
            return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.x, tq.TargetTower.x, tq.Assistingfloat1);
        }
        return false;
    }

    public static bool GetDownLeftTower(TowerPositionQuery tq) {
        Vector2 pos = new Vector2(tq.ThisTower.x - tq.Assistingfloat1, tq.ThisTower.y - tq.Assistingfloat1);
        if (tq.TargetTower.y < pos.y ) {
            if (tq.TargetTower.x < pos.x) {
                return true;
            }
        }
        return false;
    }

    public static bool GetLeftTower(TowerPositionQuery tq) {
        if (tq.TargetTower.x < tq.ThisTower.x - (tq.Assistingfloat1 / 2)) {
            return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.y, tq.TargetTower.y, tq.Assistingfloat1);
        }
        return false;
    }

    public static bool GetUpLeftTower(TowerPositionQuery tq) {
        Vector2 pos = new Vector2(tq.ThisTower.x - tq.Assistingfloat1, tq.ThisTower.y + tq.Assistingfloat1);
        if (tq.TargetTower.y > pos.y) {
            if (tq.TargetTower.x < pos.x) {
                return true;
            }
        }
        return false;
    }
    


    public static float FindAngleBetweenTwoObjects(Vector2 myPosition, Vector2 targetPosition){
    Vector2 directon = targetPosition - myPosition;
     float value = (float)((Mathf.Atan2(directon.y, directon.x) / Math.PI) * 180f);
     if(value < 0) value += 360f;
     return value;
 }



}
