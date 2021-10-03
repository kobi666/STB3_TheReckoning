using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerUtils : MonoBehaviour
{
    

    public static TowerSlotActions DefaultSlotActions = new TowerSlotActions(null,null,null,null);

    public static void PlaceTowerInSlot(TowerItemLegacy tower, GameObject TargetSlotObject, GameObject towerSlotParent) {
        TargetSlotObject = Instantiate(tower.TowerPrefab,towerSlotParent.transform.position, Quaternion.identity, towerSlotParent.transform);
        TargetSlotObject.name = (tower.TowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
    }

    public static GameObject PlaceTowerInSlotGO(TowerItemLegacy tower, GameObject towerSlotParent) {
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

    public static Dictionary<Vector2, TowerSlotController> TowerSlotControllersWithPositionsFromParent(
        TowerSlotParentManager towerSlotParentManager)
    {
        Dictionary<Vector2, TowerSlotController> allTowersUnderParentObject = new Dictionary<Vector2, TowerSlotController>();
        for (int i = 0 ; i < towerSlotParentManager.transform.childCount ; i++) {
            
            if (allTowersUnderParentObject.ContainsKey((Vector2)towerSlotParentManager.transform.GetChild(i).transform.position)) {
                continue;
            }
            if (!towerSlotParentManager.transform.GetChild(i).CompareTag("TowerSlot")) {
                continue;
            }
            if (towerSlotParentManager.transform.GetChild(i).gameObject.activeSelf == false) {
                continue;
            }

            string childName = towerSlotParentManager.transform.GetChild(i).gameObject.name;
            allTowersUnderParentObject.Add(towerSlotParentManager.TowerslotControllers[childName].Item1,towerSlotParentManager.TowerslotControllers[childName].Item2);

        }
        return allTowersUnderParentObject;
    }

public static Dictionary<Vector2, TowerPositionData> CardinalTowersNoAnglesLoop(TowerSlotController self, Dictionary<Vector2, TowerSlotController> allTowers, CardinalSet cardinalSet) {
    Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
    Vector2 selfPosition = self.transform.position;
    float towerDiscoveryRange = StaticObjects.Instance.TowerSize;
    float SecondTowerDiscoveryRange = SelectorTest2.instance.SecondDiscoveryRange;
    float RangeMultiplier = SelectorTest2.instance.SecondDiscoveryRangeMultiplier;
    
    for (int i =0 ; i < cardinalSet.length ; i++ ) {
        dict.Add(cardinalSet.directionsClockwise[i], new TowerPositionData(null as TowerSlotController, 999f));
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
                        dict[cardinalSet.directionsClockwise[i]] = new TowerPositionData(item.Value, d);
                    }
                }
            }
            
    }
    
    return dict;
    }


public static float GetDistanceScore( float baseDiscoveryRange, TowerPositionData towerPos)
{
    float distanceInDistanceUnits = (towerPos.Distance / baseDiscoveryRange);
    float DistanceScore = distanceInDistanceUnits + ((towerPos.DiscoveryRangeCycleNumber * baseDiscoveryRange) * StaticObjects.Instance.DistanceScoreMultiplier);
    return DistanceScore;
}

public static Dictionary<Vector2, TowerPositionData> CardinalTowersNoAnglesLoopOver(GameObject self,  (Vector2,TowerSlotController)[] allTowers, CardinalSet cardinalSet, int rangeCheckCyclesAmount) {
    Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
    Vector2 selfPosition = self.transform.position;
    float towerDiscoveryRange = StaticObjects.Instance.TowerSize;

    for (int i =0 ; i < cardinalSet.length ; i++ ) {
        dict.Add(cardinalSet.directionsClockwise[i], new TowerPositionData(null, 999f, 99));
        foreach (var tower in allTowers)
        {
            if (tower.Item2.name == self.name || tower.Item2 == null) {
                continue;
            }
            for (int ii = 1; ii < rangeCheckCyclesAmount; ii++)
            {
                float newDiscoveryRange = (towerDiscoveryRange * (float)ii);
                TowerPositionQuery tq = new TowerPositionQuery(selfPosition, tower.Item1, towerDiscoveryRange, newDiscoveryRange);
                if (tower.Item2.name == dict[cardinalSet.directionsClockwise[i]].TowerSlotGo?.name)
                {
                    continue;
                }
                if (cardinalSet.discoveryConditionsClockwise[i](tq))
                {
                    float d = Vector2.Distance(selfPosition, tower.Item1);
                    TowerPositionData tempTowerPos = new TowerPositionData(tower.Item2.gameObject, tower.Item2, d, ii);
                    float currentDistanceScore =
                        GetDistanceScore(towerDiscoveryRange, dict[cardinalSet.directionsClockwise[i]]);
                    float candidateDistanceScore = GetDistanceScore(towerDiscoveryRange, tempTowerPos);
                    
                    if (candidateDistanceScore < currentDistanceScore)
                    {
                        dict[cardinalSet.directionsClockwise[i]] = tempTowerPos;
                    }
                }
            }
        }
        
        
    }

    return dict;
    }
    


public static Dictionary<Vector2, TowerPositionData> CardinalTowersNoAngles(GameObject self, Dictionary<Vector2, GameObject> allTowers, CardinalSet cardinalSet, float towerDiscoveryRange) {
    Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
    Vector2 selfPosition = self.transform.position;
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
    float halfTowerSize = posRange / 2;
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

public static bool FindIfTowerInVerticalRange(TowerPositionQuery tq)
{
    float maxX = tq.ThisTower.x + tq.BaseDiscoveryRange;
    float minX = tq.ThisTower.x - tq.BaseDiscoveryRange;
    if (tq.TargetTower.x <= maxX && tq.TargetTower.x >= minX)
    {
        return true;
    }
    return false;
}

public static bool FindIfTowerInHorizontalRange(TowerPositionQuery tq)
{
    float maxY = tq.ThisTower.y + tq.BaseDiscoveryRange;
    float minY = tq.ThisTower.y - tq.BaseDiscoveryRange;
    if (tq.TargetTower.x <= maxY && tq.TargetTower.x >= minY)
    {
        return true;
    }
    return false;
}


 




    public static Vector2[] GetDirections(int numOfDirections) {
        switch (numOfDirections) {
            case 4 :
                return DirectionsClockwise4;
            case 6 :
                return DirectionsClockwise6;
            case 8 :
                return DirectionsClockwise8;
            default:
                Debug.LogWarning("No Direction set defined for " + numOfDirections);
                break;
        }
        return new Vector2[0];
    }
    public static Vector2[] DirectionsClockwise8 = { new Vector2(0,1), new Vector2(1,1),new Vector2(1,0),new Vector2(1,-1),new Vector2(0,-1),new Vector2(-1,-1),new Vector2(-1,0),new Vector2(-1,1)};
    public static Vector2[] DirectionsClockwise4 = { new Vector2(0,1),new Vector2(1,0),new Vector2(0,-1),new Vector2(-1,0)};
    public static Vector2[] DirectionsClockwise6 = {new Vector2(1,1),new Vector2(1,0),new Vector2(1,-1),new Vector2(-1,-1),new Vector2(-1,0),new Vector2(-1,1)};
    public static float[] AnglesClockwise8 = {90,45,360,315,270,225,180,135};
    public static float[] AnglesClockwise4 = {90,360,270,180};
    public static string[] DirectionNamesClockWise8 = {"UP","UP_RIGHT","RIGHT","DOWN_RIGHT","DOWN","DOWN_LEFT","LEFT","UP_LEFT"};
    public static string[] DirectionNamesClockWise4 = {"UP","RIGHT","DOWN","LEFT"};
    public static Predicate<TowerPositionQuery>[] DiscoveryConditions8 = {GetUpTower, GetUpRightTower,GetRightTower,GetDownRightTower,GetDownTower,GetDownLeftTower,GetLeftTower,GetUpLeftTower};


    public static ButtonDirectionsNames[] ButtonDirectionsNamesArray =
    {
        ButtonDirectionsNames.North,
        ButtonDirectionsNames.East,
        ButtonDirectionsNames.South,
        ButtonDirectionsNames.West
    };
    

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
        
        if (tq.TargetTower.y > tq.ThisTower.y + tq.BaseDiscoveryRange) {
                return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.x, tq.TargetTower.x, tq.DiscoveryRange );
                
        }
        return false;
    }
    public static bool GetUpRightTower(TowerPositionQuery tq) {
        Vector2 pos = new Vector2(tq.ThisTower.x + tq.DiscoveryRange , tq.ThisTower.y + tq.DiscoveryRange );
            if (tq.TargetTower.y > pos.y)
            {
                if (tq.TargetTower.x > pos.x)
                {
                    return true;
                }
            }

            return false;
    }

    public static bool GetRightTower(TowerPositionQuery tq) {
        
            if (tq.TargetTower.x > tq.ThisTower.x + tq.BaseDiscoveryRange )
            {
                return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.y, tq.TargetTower.y, tq.DiscoveryRange);
            }
        

        return false;
    }

    public static bool GetDownRightTower(TowerPositionQuery tq) {
        
            Vector2 pos = new Vector2(tq.ThisTower.x + tq.BaseDiscoveryRange, tq.ThisTower.y - tq.BaseDiscoveryRange );
            if (tq.TargetTower.y < pos.y)
            {
                if (tq.TargetTower.x > pos.x)
                {
                    return true;
                }
            }
        

        return false;
    }

    public static bool GetDownTower(TowerPositionQuery tq) {
        
            if (tq.TargetTower.y < tq.ThisTower.y - tq.BaseDiscoveryRange)
            {
                return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.x, tq.TargetTower.x, tq.DiscoveryRange );
                
            }
        
        return false;
    }

    public static bool GetDownLeftTower(TowerPositionQuery tq) {
        
            Vector2 pos = new Vector2(tq.ThisTower.x - tq.BaseDiscoveryRange , tq.ThisTower.y - tq.BaseDiscoveryRange );
            if (tq.TargetTower.y < pos.y)
            {
                if (tq.TargetTower.x < pos.x)
                {
                    return true;
                }
            }
        

        return false;
    }

    public static bool GetLeftTower(TowerPositionQuery tq) {
        
            if (tq.TargetTower.x < tq.ThisTower.x - tq.BaseDiscoveryRange)
            {
                return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.y, tq.TargetTower.y, tq.DiscoveryRange );
                
            }
        

        return false;
    }

    public static bool GetUpLeftTower(TowerPositionQuery tq) {
        
            Vector2 pos = new Vector2(tq.ThisTower.x - tq.BaseDiscoveryRange , tq.ThisTower.y + tq.BaseDiscoveryRange );
            if (tq.TargetTower.y > pos.y)
            {
                if (tq.TargetTower.x < pos.x)
                {
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
